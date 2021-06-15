using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services;

namespace ComputerInfoWebService
{
    /// <summary>
    /// Summary description for SystemInfoService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SystemInfoService : WebService
    {
        [WebMethod]
        public bool GetInfo(bool computerStatus)
        {
            return computerStatus;
        }
    }
}
