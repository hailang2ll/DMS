using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Authorizations.Model
{
    public class Permissions
    {
        public const string Name = "Permission";

        /// <summary>
        /// 测试网关授权
        /// 可以使用Blog.Core项目中的test用户
        /// 账号：test
        /// 密码：test
        /// </summary>
        public const string GWName = "GW";

        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }
}
