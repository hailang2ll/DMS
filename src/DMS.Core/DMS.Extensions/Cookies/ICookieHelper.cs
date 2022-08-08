using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Cookies
{
    public interface ICookieHelper
    {
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void SetCookie(string key, string value);

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">失效时间</param>
        void SetCookie(string key, string value, int expires);
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="options"></param>
        void SetCookie(string key, string value, CookieOptions options);
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>Cookie值</returns>
        string GetCookie(string key);

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key">键</param>
        void DeleteCookie(string key);
    }
}
