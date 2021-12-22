using DMS.Common;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DMS.XUnitTest
{
    public class Appsettings_Test : Base_Test
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Test()
        {
#if NETCOREAPP3_1 || NET5_0
            var memberUrl = AppConfig.GetValue("MemberUrl");
            var getConnectionString = AppConfig.GetConnectionString("trydou_sys_master");

            var VALIDATECODE_LENGTH = AppConfig.GetValue(new string[] { "SmsSetting", "VALIDATECODE_LENGTH" });
            List<MutiDBOperate> listdatabase = AppConfig.GetValue<List<MutiDBOperate>>("DBS");
            List<MutiDBOperate> listdatabase2 = AppConfig.GetValueList<MutiDBOperate>("DBS")
                .Where(i => i.Enabled).ToList(); ;
#endif
        }
    }

    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4,
        Dm = 5,
        Kdbndp = 6,
    }
    public class MutiDBOperate
    {
        /// <summary>
        /// 连接启用开关
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }
        /// <summary>
        /// 从库执行级别，越大越先执行
        /// </summary>
        public int HitRate { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }
    }
}
