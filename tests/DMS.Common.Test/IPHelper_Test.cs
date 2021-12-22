using DMS.Common.Helper;
using NUnit.Framework;

namespace DMS.Common.Test
{
    public class IPHelper_Test
    {
        [Test(Description = "")]
        public void GetIP()
        {
            //如果.netcore获取IP，必须先注册才能使用 services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var ip = IPHelper.GetCurrentIp();
        }
    }
}
