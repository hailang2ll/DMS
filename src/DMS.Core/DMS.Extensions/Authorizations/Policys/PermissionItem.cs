using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Authorizations.Policys
{
    /// <summary>
    /// 用户权限承载实体
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}
