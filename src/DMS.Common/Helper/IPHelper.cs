using DMS.Common.Configurations;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DMS.Common.Helper
{
    /// <summary>
    /// SERVER获取IP的帮助类
    /// 获取客户端IP,服务器端IP
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 获取DNSIP
        /// </summary>
        /// <returns></returns>
        public static string GetServerDnsIP()
        {
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取web客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIp()
        {
            string userIP = "0.0.0.0";
            if (RegisterHttpContextExtensions.Current == null || RegisterHttpContextExtensions.Current.Request == null)
                return userIP;
            string CustomerIP = "";
            //CustomerIP = MyHttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"];
            //if (!string.IsNullOrEmpty(CustomerIP))
            //    return CustomerIP;

            //CustomerIP = MyHttpContext.Current.Request.Headers["REMOTE_ADDR"];
            //if (!String.IsNullOrEmpty(CustomerIP))
            //    return CustomerIP;

            //CustomerIP = MyHttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"];
            //if (!String.IsNullOrEmpty(CustomerIP))
            //    return CustomerIP;

            CustomerIP = RegisterHttpContextExtensions.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(CustomerIP))
            {
                CustomerIP = RegisterHttpContextExtensions.Current.Connection.RemoteIpAddress.ToString();
            }
            return CustomerIP;

        }

    }
}
