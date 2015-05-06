using BookMeeting2.Models;
using INXService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BookMeeting2.Common
{
    public class APIHelper
    {

        public IEnumerable<ModelMR> GetMeetingRoom(string site, string plant, string floor)
        {
            string sSite = site == "" || site == null ? " 1=1 " : " b.site = '" + site + "'";
            string sPlant = plant == "" || plant == null ? " 1=1 " : " b.plant = '" + plant + "'";
            string sFloor = floor == "" || floor == null ? " 1=1 " : " b.floor = '" + floor + "'";

            IEnumerable<ModelMR> ret = null;
            string sql = string.Format(@"select a.roomid,
                   a.roomname,
                   a.roomabbr,
                   a.siteid,
                   b.site,
                   b.plant,
                   b.floor,
                   a.eqipment,
                   a.ext,
                   a.cnpc,
                   a.ip,
                   a.accessright,
                   a.postperiod,
                   a.imgpath,
                   a.minp,
                   a.maxp
              from MBS_ROOM a, mbs_area b
             where a.siteid = b.siteid
              and {0}
              and {1} 
              and {2}
              order by roomname ", sSite, sPlant, sFloor);

            DataSet ds = DALService.ExecuteDataSet(sql);

            return GetMRList(ds);

        }
        public ModelMR GetMeetingRoomByID(string id)
        {
            string sql = string.Format(@"select a.roomid,
                   a.roomname,
                   a.roomabbr,
                   a.siteid,
                   b.site,
                   b.plant,
                   b.floor,
                   a.eqipment,
                   a.ext,
                   a.cnpc,
                   a.ip,
                   a.accessright,
                   a.postperiod,
                   a.imgpath,
                   a.minp,
                   a.maxp
              from MBS_ROOM a, mbs_area b
             where a.siteid = b.siteid
              and roomid ='{0}'
              order by roomname", id);

            DataSet ds = DALService.ExecuteDataSet(sql);

            return ds.Tables[0].Rows.Count == 1 ? GetMRList(ds).First() : null;

           //return GetMRList(ds);

        }

        private IEnumerable<ModelMR> GetMRList(DataSet ds)
        {
            return ds.Tables[0].AsEnumerable().Select(x => new ModelMR
            {
                ROOMID = x.IsNull("ROOMID") ? "" : x["ROOMID"].ToString(),
                ROOMNAME = x.IsNull("ROOMNAME") ? "" : x["ROOMNAME"].ToString(),
                ROOMABBR = x.IsNull("ROOMABBR") ? "" : x["ROOMNAME"].ToString(),
                SITEID = x.IsNull("SITEID") ? "" : x["SITEID"].ToString(),
                SITE = x.IsNull("SITE") ? "" : x["SITE"].ToString(),
                PLANT = x.IsNull("PLANT") ? "" : x["PLANT"].ToString(),
                FLOOR = x.IsNull("FLOOR") ? "" : x["FLOOR"].ToString(),
                EQIPMENT = x.IsNull("EQIPMENT") ? "" : x["EQIPMENT"].ToString(),
                EXT = x.IsNull("EXT") ? "" : x["EXT"].ToString(),
                CNPC = x.IsNull("CNPC") ? "" : x["CNPC"].ToString(),
                IP = x.IsNull("IP") ? "" : x["IP"].ToString(),
                ACCESSRIGHT = x.IsNull("ACCESSRIGHT") ? "" : x["ACCESSRIGHT"].ToString(),
                POSTPERIOD = x.IsNull("POSTPERIOD") ? 0 : x["POSTPERIOD"].ToString() == "" ? 0 : int.Parse(x["POSTPERIOD"].ToString()),
                IMGPATH = x.IsNull("IMGPATH") ? "" : x["IMGPATH"].ToString(),
                MINP = x.IsNull("MINP") ? 0 : x["MINP"].ToString() == "" ? 0 : int.Parse(x["MINP"].ToString()),
                MAXP = x.IsNull("MAXP") ? 0 : x["MAXP"].ToString() == "" ? 0 : int.Parse(x["MAXP"].ToString()),
            })
            ;
        }

        public bool UpdateMRRoom(ModelMR mr) {

            clsSQLBuilder cls = new clsSQLBuilder();

            cls.AddField("ACCESSRIGHT", clsSQLBuilder.FieldType.FD_STRING, mr.ACCESSRIGHT, true);
            cls.AddField("CNPC", clsSQLBuilder.FieldType.FD_STRING, mr.CNPC, true);
            cls.AddField("EQIPMENT", clsSQLBuilder.FieldType.FD_STRING, mr.EQIPMENT, true);
            cls.AddField("EXT", clsSQLBuilder.FieldType.FD_STRING, mr.EXT, true);
           // cls.AddField("FLOOR", clsSQLBuilder.FieldType.FD_STRING, mr.FLOOR, true);
            cls.AddField("IMGPATH", clsSQLBuilder.FieldType.FD_STRING, mr.IMGPATH, true);
            cls.AddField("IP", clsSQLBuilder.FieldType.FD_STRING, mr.IP, true);
            cls.AddField("MAXP", clsSQLBuilder.FieldType.FD_NUMBER, mr.MAXP.ToString(), true);

            cls.AddField("MINP", clsSQLBuilder.FieldType.FD_NUMBER, mr.MINP.ToString(), true);
            cls.AddField("POSTPERIOD", clsSQLBuilder.FieldType.FD_NUMBER, mr.POSTPERIOD.ToString(), true);
            cls.AddField("PLANT", clsSQLBuilder.FieldType.FD_STRING, mr.PLANT.ToString(), true);
            cls.AddField("ROOMABBR", clsSQLBuilder.FieldType.FD_STRING, mr.ROOMABBR.ToString(), true);
            cls.AddField("ROOMID", clsSQLBuilder.FieldType.FD_STRING, mr.ROOMID.ToString(), false);
            cls.AddField("ROOMNAME", clsSQLBuilder.FieldType.FD_STRING, mr.ROOMNAME.ToString(), true);
            cls.AddField("SITE", clsSQLBuilder.FieldType.FD_STRING, mr.SITE.ToString(), true);
            cls.AddField("SITEID", clsSQLBuilder.FieldType.FD_STRING, GetSiteid(mr.SITE,mr.PLANT,mr.FLOOR), true);

            string sqls = string.Format(@"MERGE INTO {0} USING DUAL
                    ON (ROOMID = '{1}')
                    WHEN MATCHED THEN
                        UPDATE SET {2}
                    WHEN NOT MATCHED THEN
                        INSERT {3}"
                    , "MBS_ROOM "
                    , mr.ROOMID
                    , cls.GetString_Update()
                    ,cls.GetString_Insert()
                );


            DALService.ExecuteNonQuery(sqls);
            return true;
        }

        private string GetSiteid(string site, string plant, string floor)
        {
            string sql = string.Format(@"
                select siteid from MBS_AREA t where t.site = '{0}' and t.plant = '{1}' and t.floor = '{2}'
             ", site, plant, floor);
            return DALService.ExecuteDataSet(sql).Tables[0].Rows[0][0].ToString();
        }

        public IEnumerable<ModelRecord> GetBookingData(string sdate, string userid, List<string> roomid)
        {
            List<string> sqls = new List<string>();
            DateTime dt1 = DateTime.Parse(sdate);
            string dayofWeek = ((int)dt1.DayOfWeek).ToString();

            // 一般預約
            sqls.Add(string.Format(@"
                select t.roomid,
                   t.RECORDID,
                   t.starttime,
                   t.endtime,
                   decode(lower(t.applierid),'{2}','APPL',t.booktype) booktype,
                   t.updatet,
                   t.applierid,
                   t.applier,
                   t.applierext,
                   '' as dateday
              from MBS_RECORD t
                 where t.starttime <= to_date('{0}', 'yyyy/mm/dd') + 1
                   and t.endtime >= to_date('{0}', 'yyyy/mm/dd')
                   and t.roomid in ('{1}')
            ", sdate, string.Join("','", roomid.ToArray()), userid));

            // 系統預約
//            sqls.Add(string.Format(@"
//            select t.roomid,
//                   t.sid as RECORDID,
//                   to_date('{0} ' || SUBSTR(starttime, 1, 2) || ':' ||
//                           SUBSTR(starttime, 3, 2) || ':' || '00',
//                           'yyyy/mm/dd hh24:mi:ss') as starttime,
//                   to_date('{0} ' || SUBSTR(endtime, 1, 2) || ':' ||
//                           SUBSTR(endtime, 3, 2) || ':' || '00',
//                           'yyyy/mm/dd hh24:mi:ss') as endtime,
//                   sysdate as UPDATET,
//                   'SYSL' as booktype,
//                   'SYSTEM' as APPLIER,
//                   'SYSTEM' as applierid,
//                   '' as applierext,
//                    dateday
//              from MBS_LOCKROOM t
//             where t.roomid in ('{1}')
//               and t.dateday = '{2}'
//            ", sdate, string.Join("','", roomid.ToArray()), dayofWeek));


            DataSet ds = DALService.ExecuteBatchQuery(sqls.ToArray());

            IEnumerable<ModelRecord> ret = GetBRecordList(ds.Tables[0]);

            ret = ret.Concat(
                GenRangeSysBookRecord(roomid, sdate, sdate));
            ds.Dispose();
            return ret;
        }

        public IEnumerable<ModelRecord> GetBookingData(string Roomid,string sdate, string edate, string userid)
        {

            string sql = string.Format(@"
                select t.roomid,
                   t.RECORDID,
                   t.starttime,
                   t.endtime,
                   decode(lower(t.applierid),'{3}','APPL',t.booktype) booktype,
                   t.updatet,
                   t.applierid,
                   t.applier,
                   t.applierext,
                   '' as dateday
              from MBS_RECORD t
                 where t.starttime between to_date('{0}', 'yyyy/mm/dd')  and to_date('{1}', 'yyyy/mm/dd')+1
                   and t.roomid in ('{2}')
            ", sdate,edate, Roomid, userid);

            DataSet ds = DALService.ExecuteDataSet(sql);
            IEnumerable<ModelRecord> ret = GetBRecordList(ds.Tables[0]);

            ret = ret.Concat(
                GenRangeSysBookRecord(new List<string> { Roomid }, sdate, edate));
            ds.Dispose();
            return ret;
        }

        private  IEnumerable<ModelRecord>  GenRangeSysBookRecord(List<string> roomid,string sdate, string edate){
            List<ModelRecord> ret = new List<ModelRecord>();
            string sql = string.Format(@"
            select t.roomid,
                   t.sid as RECORDID,
                   to_date('{0} ' || SUBSTR(starttime, 1, 2) || ':' ||
                           SUBSTR(starttime, 3, 2) || ':' || '00',
                           'yyyy/mm/dd hh24:mi:ss') as starttime,
                   to_date('{0} ' || SUBSTR(endtime, 1, 2) || ':' ||
                           SUBSTR(endtime, 3, 2) || ':' || '00',
                           'yyyy/mm/dd hh24:mi:ss') as endtime,
                   sysdate as UPDATET,
                   'SYSL' as booktype,
                   'SYSTEM' as APPLIER,
                   'SYSTEM' as applierid,
                   '' as applierext,
                  dateday
              from MBS_LOCKROOM t
             where t.roomid in ('{1}')
            ", sdate, 
             string.Join("','", roomid.ToArray()));

            IEnumerable<ModelRecord> ret2 = GetBRecordList(DALService.ExecuteDataSet(sql).Tables[0]);

            DateTime dts1 = DateTime.Parse(sdate);
            DateTime dts = DateTime.Parse(sdate);
            DateTime dte = DateTime.Parse(edate);

            int i = 0;
            
            while(DateTime.Compare(dte, dts)>=0){
                IEnumerable<ModelRecord> ret3 = ret2
                                                .Where(x => x.DATEDAY == ((int)dts.DayOfWeek).ToString())
                                                .Select(x => x);
                if (ret3.Count() > 0) {
                    ret3 = ret3.Select(x =>
                    {
                        x.STARTTIME = DateTime.Parse(x.STARTTIME).AddDays(i).ToString("yyyy/MM/dd HH:mm:ss");
                        x.ENDTIME = DateTime.Parse(x.ENDTIME).AddDays(i).ToString("yyyy/MM/dd HH:mm:ss");
                        return x;
                    });
                    ret.AddRange(ret3.ToList());
                }
                i = i + 1;
                dts = dts1.AddDays(i);
            }

            
            return ret;
        }



        private IEnumerable<ModelRecord> GetBRecordList(DataTable dt)
        {
            return dt.AsEnumerable().Select(x => new ModelRecord
            {
                RECORDID = x.IsNull("RECORDID") ? "" : x["RECORDID"].ToString(),
                ROOMID = x.IsNull("ROOMID") ? "" : x["ROOMID"].ToString(),
                //  ROOMNAME = x.IsNull("ROOMNAME") ? "" : x["ROOMNAME"].ToString(),
                STARTTIME = x.IsNull("STARTTIME") ? "" : (DateTime.Parse(x["STARTTIME"].ToString())).ToString("yyyy/MM/dd HH:mm:ss"),
                ENDTIME = x.IsNull("ENDTIME") ? "" : (DateTime.Parse(x["ENDTIME"].ToString())).ToString("yyyy/MM/dd HH:mm:ss"),
                APPLIER = x.IsNull("APPLIER") ? "" : x["APPLIER"].ToString(),
                APPLIEREXT = x.IsNull("APPLIEREXT") ? "" : x["APPLIEREXT"].ToString(),
                APPLIERID = x.IsNull("APPLIERID") ? "" : x["APPLIERID"].ToString(),
                BOOKTYPE = x.IsNull("BOOKTYPE") ? "" : x["BOOKTYPE"].ToString(),
                UPDATET = x.IsNull("UPDATET") ? "" : (DateTime.Parse(x["UPDATET"].ToString())).ToString("yyyy/MM/dd HH:mm:ss"),
                DATEDAY = x.IsNull("DATEDAY") ? "" : x["DATEDAY"].ToString()
            })
            ;
        }

        // 操作
        public int DeleteBookingData(string recordid, string userid)
        {
            string sql = string.Format(@"
                delete
               MBS_RECORD t
                 where recordid = '{0}'
                        and applierid = '{1}'
                        and starttime > sysdate
                ", recordid, userid);
            return DALService.ExecuteNonQuery(sql);
        }


        public void BookingEvent(ModelRecord[] objM) {
            // 更新Room
            List<string> sqls = new List<string>();
            foreach(var ob in objM){
                clsSQLBuilder cls = new clsSQLBuilder();
                cls.AddField("RECORDID", clsSQLBuilder.FieldType.FD_STRING, ob.RECORDID, false);
                cls.AddField("ROOMID", clsSQLBuilder.FieldType.FD_STRING, ob.ROOMID, false);
                cls.AddField("STARTTIME", clsSQLBuilder.FieldType.FD_DATETIME, ob.STARTTIME, false);
                cls.AddField("ENDTIME", clsSQLBuilder.FieldType.FD_DATETIME, ob.ENDTIME, false);
                cls.AddField("BOOKTYPE", clsSQLBuilder.FieldType.FD_STRING, ob.BOOKTYPE, false);
                cls.AddField("UPDATET", clsSQLBuilder.FieldType.FD_DATE, ob.UPDATET, false);
                cls.AddField("APPLIER", clsSQLBuilder.FieldType.FD_STRING, ob.APPLIER, false);
                cls.AddField("APPLIEREXT", clsSQLBuilder.FieldType.FD_STRING, ob.APPLIEREXT, false);
                cls.AddField("APPLIERID", clsSQLBuilder.FieldType.FD_STRING, ob.APPLIERID, false);
                cls.AddField("RECORDID", clsSQLBuilder.FieldType.FD_STRING, ob.RECORDID, false);

                sqls.Add(string.Format(@" insert into MBS_RECORD {0}",cls.GetString_Insert()));
            }
            DALService.ExecuteBatchNonQuery(sqls.ToArray());
        
        }
    }
}