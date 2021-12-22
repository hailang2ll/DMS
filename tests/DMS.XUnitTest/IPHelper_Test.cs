using DMS.Common.Helper;
using Xunit;

namespace DMS.XUnitTest
{
    public class IPHelper_Test
    {
        [Fact]
        public void GetIP()
        {
            //如果.netcore获取IP，必须先注册才能使用 services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var ip = IPHelper.GetCurrentIp();
        }
    }
}
