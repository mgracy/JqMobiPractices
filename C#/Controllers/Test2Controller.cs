using INXService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BookMeeting2.Controllers
{
    public class Test2Controller : ApiController
    {

        [HttpGet]
        public IEnumerable<string> GetMeetingRoom(string ConsumerKey, string ConsumerSecret)
        {
            UserInfo  vUserInfo = new UserInfo();
            return new string[] { vUserInfo.AD, vUserInfo.UserId };
        }


    }
}
