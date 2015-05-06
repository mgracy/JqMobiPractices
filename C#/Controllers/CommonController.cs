using DotNet.Utilities;
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
    public class CommonController : ApiController
    {

        [HttpGet]
        public IEnumerable<DataRow> GetMenu()
        {
            UserInfo vUserInfo = new UserInfo();
            INXService.SAMService ss = new INXService.SAMService("MBooking","10131390");
       //     DataTable dtRet = ss.GetSystemFunctions();
            DataTable  dtRet = ss.GetMenu();

      
            DataTable dtCopy = dtRet.Copy();

            DataView dv = dtRet.DefaultView;
            dv.Sort = "SORTINDEX";
            dtCopy = dv.ToTable();
            dtRet.Dispose();

            return dtCopy.AsEnumerable();
        }

        [HttpGet]
        public string ChangeRate(string Rate)
        {
            long out1;
            DownLoadHelper.Speed = long.TryParse(Rate, out out1) ? long.Parse(Rate) : 1024;
            return "";
        }

    }
}
