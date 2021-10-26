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
        /// 简单token认证
        /// </summary>
        Token,
        /// <summary>
        /// 基于auth2.0认证
        /// </summary>
        Auth20
    }
}
