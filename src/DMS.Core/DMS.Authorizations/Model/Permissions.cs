using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.Model
{
    public class Permissions
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        public const string Name = "Permisson";

        /// <summary>
        /// 当前项目是否启用IDS4权限方案
        /// true：表示启动IDS4
        /// false：表示使用JWT
        public static bool IsUseIds4 = false;
    }
}
