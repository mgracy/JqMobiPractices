using BookMeeting2.Common;
using BookMeeting2.Models;
using INXService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookMeeting2.Controllers
{
    public class EventController : ApiController
    {

        [HttpPost]
        public IEnumerable<ModelRecord> GetEventByRooms([FromBody] ModelComplex mrdata)
        {
            APIHelper ws = new APIHelper();
            UserInfo vUserInfo = new UserInfo();

            string userAD = vUserInfo.UserId.ToLower();

            List<ModelRecord> lis = ws.GetBookingData(
                mrdata.sdate, userAD,
                mrdata.mrdata.Select(x => x.ROOMID).ToList()).ToList<ModelRecord>();
            return lis;
        }

        [HttpGet]
        public IEnumerable<ModelRecord> GetEventByRoomDate(string roomid, string sDate, string eDate)
        {
            APIHelper ws = new APIHelper();
            UserInfo vUserInfo = new UserInfo();

            string userAD = vUserInfo.UserId.ToLower();

            List<ModelRecord> lis = ws.GetBookingData(roomid, sDate, eDate, userAD).ToList<ModelRecord>();

            return lis;
        }

        [HttpGet]
        public IEnumerable<ModelRecord> GetFreeEventByRoom(string site, string plant, string loc, string stime)
        {
            APIHelper ws = new APIHelper();
            UserInfo vUserInfo = new UserInfo();
            string userAD = vUserInfo.UserId.ToLower();

            // 取得有權限的ROOM
            List<ModelMR> lis = ws.GetMeetingRoom(site, plant, loc).ToList<ModelMR>();

            HR ff = new HR();
            DataSet ds = ff.GetManager(userAD);
            List<string> DeptArray = ds.Tables[0].AsEnumerable().Select(x => x["DEPTID"].ToString()).ToList();
            ds.Dispose();
            DeptArray.Add(userAD);
            lis = lis.Where(x =>
                x.ACCESSRIGHT == "" || DeptArray.Any(
                                          y => x.ACCESSRIGHT.Contains(y))
                ).Select(x => x).ToList<ModelMR>();

            DateTime dts = DateTime.Parse(stime);
            string sdate = dts.ToString("yyyy/MM/dd");


            // 取得結束日期 大於起始時間的記錄 
            IEnumerable<ModelRecord> RecordList = ws.GetBookingData(
                  sdate, userAD,
                  lis.Select(x => x.ROOMID).ToList())
                  .Where(x => DateTime.Compare(DateTime.Parse(x.ENDTIME), dts) > 0)
                  .ToList<ModelRecord>();

            List<ModelRecord> ret = new List<ModelRecord>();
            foreach (ModelMR mr in lis)
            {
                IEnumerable<ModelRecord> tRecord = RecordList.Where(x => x.ROOMID == mr.ROOMID).ToList();



                if (tRecord.Count()>0)
                {

                    DateTime minStartTime = RecordList.Where(x => x.ROOMID == mr.ROOMID).Select(x => DateTime.Parse(x.STARTTIME)).Min();
                    if (DateTime.Compare(dts, minStartTime) > 0)
                    {
                        // 此會議室忙碌
                    }
                    else
                    {
                        ret.Add(new ModelRecord {
                            APPLIER = "PreBooking",
                            APPLIEREXT = "PreBooking",
                            APPLIERID = "PreBooking",
                            BOOKTYPE = "PreBooking",
                            DATEDAY = dts.DayOfWeek.ToString(),
                            ENDTIME = minStartTime.ToString("yyyy/MM/dd HH:mm:ss"),
                            STARTTIME = dts.ToString("yyyy/MM/dd HH:mm:ss"),
                            ROOMNAME = mr.ROOMNAME,
                            RECORDID = Guid.NewGuid().ToString(),
                            ROOMID = mr.ROOMID
                        });
                    }
                }

                else {
                    ret.Add( new ModelRecord
                    {
                        APPLIER = "PreBooking",
                        APPLIEREXT = "PreBooking",
                        APPLIERID = "PreBooking",
                        BOOKTYPE = "PreBooking",
                        DATEDAY = dts.DayOfWeek.ToString(),
                        ENDTIME = DateTime.Parse(sdate).AddHours(22).ToString("yyyy/MM/dd HH:mm:ss"),
                        STARTTIME = dts.ToString("yyyy/MM/dd HH:mm:ss"),
                        ROOMNAME = mr.ROOMNAME,
                        RECORDID = Guid.NewGuid().ToString(),
                        ROOMID = mr.ROOMID,
                        ALLOWPERSON = mr.MINP.ToString()+" ~ "+mr.MAXP.ToString()
                    });
                }
            }
            return ret;
        }


        //public string GetEventByRooms([FromBody] string sdate, [FromBody] IEnumerable<ModelMR> mrdata)
        //{
        //    return this.Request.CreateResponse(HttpStatusCode.OK);
        //    return "rwwsss";
        //}
        [HttpPost]
        public string RemoveBook([FromBody] string RecordID)
        {
            APIHelper ws = new APIHelper();
            UserInfo vUserInfo = new UserInfo();

            string userAD = vUserInfo.UserId.ToLower();

            int ret = ws.DeleteBookingData(RecordID, userAD);




            return ret == 0 ? "刪除失敗 (無此權限)" : "刪除成功";
        }


        [HttpPost]
        public string BookRoomEvent([FromBody] ModelRecord recordobj)
        {
            string ret = "成功預約";
            DateTime out1;
            try
            {
                APIHelper ws = new APIHelper();
                UserInfo vUserInfo = new UserInfo();
                string roomid = recordobj.ROOMID;

                ModelMR mr = ws.GetMeetingRoomByID(roomid);
                if (mr == null) throw new Exception("找不到會議室");

                DateTime sdt = DateTime.Parse(recordobj.STARTTIME);
                DateTime edt = DateTime.Parse(recordobj.ENDTIME);

                // .ToString("yyyy/MM/dd HH:mm:ss"),
                List<int> minuInt = new List<int> { 0, 15, 30, 45 };
                if (!minuInt.Contains(sdt.Minute) || !minuInt.Contains(edt.Minute))
                {
                    throw new Exception("時間格式錯誤");
                }
                TimeSpan ts = edt.Subtract(sdt);

                if (ts.TotalSeconds == 0)
                {
                    throw new Exception("起始結束時間不可一樣");
                }
                if (DateTime.Compare(DateTime.Today, sdt) > 0)
                {
                    throw new Exception("預約日期不可小於今日");
                }

                // 檢查預約天數 
                if (DateTime.Compare(sdt, DateTime.Today.AddDays(mr.POSTPERIOD)) > 0)
                {
                    throw new Exception("預約日期不可大於 (" + mr.POSTPERIOD + ") 天");
                }

                // 檢查會議室時間是否為系統預約
                int dateday = (int)sdt.DayOfWeek;
                List<string> sqls = new List<string>();


                #region   檢查 是否被預約
                sqls.Add(string.Format(@"
                    select *
                      from MBS_RECORD t
                     where t.roomid = '{0}'
                       and( t.starttime between
                           to_date('{1}', 'yyyy/mm/dd hh24:mi:ss') AND
                           to_date('{2}', 'yyyy/mm/dd hh24:mi:ss')
                         or  t.endtime between
                           to_date('{1}', 'yyyy/mm/dd hh24:mi:ss') AND
                           to_date('{2}', 'yyyy/mm/dd hh24:mi:ss'))
               ", roomid, sdt.AddSeconds(1).ToString("yyyy/MM/dd HH:mm:ss"), edt.AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss")));
                #endregion
                #region 檢查系統預約 sql

                sqls.Add(string.Format(@"select a.dateday,a.starttime,a.endtime
                      from MBS_LOCKROOM a
                     where a.roomid = '{0}'
                       and (a.starttime between '{2}' AND '{3}' or
                           a.endtime between '{4}' AND '{5}')
                           and a.dateday = '{1}'
                    ", roomid
                     , dateday.ToString(),
                     sdt.ToString("HHmm"),
                     edt.AddMinutes(-1).ToString("HHmm"),
                     sdt.AddMinutes(1).ToString("HHmm"),
                     edt.ToString("HHmm")
                     ));
                #endregion

                DataSet ds = DALService.ExecuteBatchQuery(sqls.ToArray());
                IEnumerable<DataRow> drow = ds.Tables[0].AsEnumerable();
                if (drow.Count() > 0)
                {
                    string sRepeat = string.Join("\n", drow.Select(
                        x => x["applier"].ToString() + " From : " + x["starttime"].ToString() + " To :" + x["endtime"].ToString()).ToArray());

                    throw new Exception("重覆預約 \n" + sRepeat);
                }

                IEnumerable<DataRow> Srow = ds.Tables[1].AsEnumerable();
                ds.Dispose();
                if (Srow.Count() > 0)
                {
                    string sRepeat = string.Join("\n", Srow.Select(
                        x => Enum.GetName(typeof(DayOfWeek), int.Parse(x["dateday"].ToString())) + " : " + x["starttime"].ToString() + " To :" + x["endtime"].ToString()
                            ).ToArray());
                    throw new Exception("此會議室已被系統預約 時間 \n" + sRepeat);
                }




                recordobj.RECORDID = Guid.NewGuid().ToString();
                recordobj.BOOKTYPE = "USER";
                recordobj.APPLIER = vUserInfo.CName;
                recordobj.APPLIEREXT = vUserInfo.Ext;
                recordobj.APPLIERID = vUserInfo.UserId;
                recordobj.UPDATET = "sysdate";
                ws.BookingEvent(new
                    [] { recordobj });
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            //檢查預約事件 格式是否異常


            return ret;
        }
    }
}
