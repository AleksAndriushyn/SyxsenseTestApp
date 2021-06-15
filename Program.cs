using ComputerInfoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Syxsense.App
{
    class Program
    {
        static void Main(string[] args)
        {
            GetInformation();
            Console.ReadKey();
        }

        public static void GetInformation()
        {
            SelectQuery query = new SelectQuery(@"Select * from Win32_OperatingSystem");
            
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (ManagementObject process in searcher.Get())
                {
                    //print system info
                    Console.WriteLine("/*********Operating System Information ***************/");
                    Console.WriteLine("{0}{1}", "OS name: ", process["Caption"]);
                    Console.WriteLine("{0}{1}", "Time zone: ", process["CurrentTimeZone"]);
                    Console.WriteLine("{0}{1}", "Date time: ", process["LocalDateTime"]);
                    Console.WriteLine("{0}{1}", "Computer name: ", process["CSName"]);
                    //Console.WriteLine("{0}{1}", ".Net version:", process[".net version"]);

                    SystemInfoService systemInfoService = new SystemInfoService();
                    var response = systemInfoService.GetInfo(IsMachineUp(process["CSName"].ToString()));

                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromMinutes(5);

                    var timer = new System.Threading.Timer((e) =>
                    {
                        Console.WriteLine("Is machine online? {0}", response);
                    }, null, startTimeSpan, periodTimeSpan);
                }
            }
        }

        private static bool IsMachineUp(string hostName)
        {
            bool retVal = false;
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;

                PingReply reply = pingSender.Send(hostName, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                retVal = false;
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }
    }
}
