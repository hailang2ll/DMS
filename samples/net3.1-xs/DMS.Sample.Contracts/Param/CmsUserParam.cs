using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Sample.Contracts.Param
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginCmsUserParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }
    }
}
