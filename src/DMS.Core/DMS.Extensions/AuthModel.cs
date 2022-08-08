using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions
{
    /// <summary>
    /// 认证模式
    /// </summary>
    public enum AuthModel
    {
        /// <summary>
        /// cookies认证
        /// </summary>
        Cookies,
        /// <summary>
        /// token认证
        /// </summary>
        Token,
        /// <summary>
        /// jwt+redis认证
        /// </summary>
        Jwt,
        /// <summary>
        /// id4+redis认证
        /// </summary>
        Id4
    }
}
