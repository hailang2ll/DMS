using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Cookies
{
    public class CookieHelper : ICookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 通过构造函数进行注入
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 根据key值删除对应的Cookie
        /// </summary>
        /// <param name="key">key值</param>
        public void DeleteCookie(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 根据key值获取Cookie的value值
        /// </summary>
        /// <param name="key">key值</param>
        /// <returns></returns>
        public string GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        public void SetCookie(string key, string value)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value);
        }

        /// <summary>
        /// 设置Cookie及过期时间
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="expiresTime">过期时间，以分钟为单位</param>
        public void SetCookie(string key, string value, int expiresTime)
        {
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(expiresTime)
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }

        /// <summary>
        /// 设置Cookie及过期时间
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="options">CookieOptions</param>
        public void SetCookie(string key, string value, CookieOptions options)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }
    }
}
