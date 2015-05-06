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
    public class RoomController : ApiController
    {
        [HttpGet]
        public IEnumerable<ModelMR> GetMeetingRoom(string site, string plant, string loc)
        {
           APIHelper ws = new APIHelper();

            List <ModelMR> lis = ws.GetMeetingRoom(site, plant, loc).ToList<ModelMR>();

            UserInfo vUserInfo = new UserInfo();
            string userAD = vUserInfo.UserId.ToLower();
            HR ff = new HR();
            DataSet ds = ff.GetManager(userAD);

           List<string> DeptArray = ds.Tables[0].AsEnumerable().Select(x => x["DEPTID"].ToString()).ToList();
           ds.Dispose();
           DeptArray.Add(userAD);

           lis = lis.Where(x => 
               x.ACCESSRIGHT == "" || DeptArray.Any(
                                         y => x.ACCESSRIGHT.Contains(y))
               ).Select(x => x).ToList<ModelMR>();
            return lis;
        }

        [HttpGet]
        public ModelMR GetRoomInfoByID(string roomid)
        {
            APIHelper ws = new APIHelper();

            ModelMR lis = ws.GetMeetingRoomByID(roomid);
            return lis;
        }


        [HttpGet]
        public IEnumerable<string> GetSite()
        {
            APIHelper ws = new APIHelper();

            List<ModelMR> lis = ws.GetMeetingRoom("", "", "").ToList<ModelMR>();

            List<string> ret = lis.Select(x => x.SITE).Distinct().ToList();
            return ret;
        }

        [HttpGet]
        public IEnumerable<ModelMR> GetPlant(string site)
        {
            APIHelper ws = new APIHelper();

            List<ModelMR> lis = ws.GetMeetingRoom(site, "", "").ToList<ModelMR>();


            var query = from x in lis
                            group x by new
                            {
                                   SITE = x.SITE,
                                    PLANT = x.PLANT,
                                    FLOOR = x.FLOOR
                            } into outer
                            select new ModelMR
                            {
                                SITE = outer.Key.SITE,
                                PLANT = outer.Key.PLANT,
                                FLOOR = outer.Key.FLOOR
                            }
                            ;
            query = query.OrderBy(x => x.PLANT + "_" + x.FLOOR);
            return query.ToList<ModelMR>();
        }

    }
}
