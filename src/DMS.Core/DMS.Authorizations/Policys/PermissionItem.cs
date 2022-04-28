using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.Policys
{
    /// <summary>
    /// 用户权限承载实体
    /// </summary>
    public class PermissionItem
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}
