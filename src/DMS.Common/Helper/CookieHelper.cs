using DMS.Common.Configurations;
using DMS.Common.Encrypt;
using DMS.Common.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Common.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class CookieHelper
    {
        #region EscapeHelper
        public static void SetEscapeCookie(string strName, string strValue, int strDay = 0)
        {
            CookieOptions options = new CookieOptions();
            options.Domain = DominName;
            if (strDay > 0)
            {
                options.Expires = DateTimeOffset.Now.AddDays(strDay);
            }
            options.IsEssential = true;
            RegisterHttpContextExtensions.Current.Response.Cookies.Append(strName, EscapeHelper.escape(strValue), options);
        }

        public static string GetEscapeCookie(string strName)
        {
            var strValue = RegisterHttpContextExtensions.Current.Request.Cookies[strName];
            if (!string.IsNullOrWhiteSpace(strValue) && strValue.Length > 0)
            {
                return EscapeHelper.unescape(strValue);
            }
            return "";
        }
        #endregion

        #region DESHelper
        public static void SetSOSCookie(string strName, string strValue, int strDay = 0)
        {
            CookieOptions options = new CookieOptions();
            options.Domain = DominName;
            if (strDay > 0)
            {
                options.Expires = DateTimeOffset.Now.AddDays(strDay);
            }
            options.IsEssential = true;
            RegisterHttpContextExtensions.Current.Response.Cookies.Append(strName, DESHelper.Encode(strValue), options);
        }

        public static string GetSOSCookie(string strName)
        {
            var strValue = RegisterHttpContextExtensions.Current.Request.Cookies[strName];
            if (!string.IsNullOrWhiteSpace(strValue) && strValue.Length > 0)
            {
                return DESHelper.Decode(strValue);
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 删除cookies
        /// </summary>
        /// <param name="strName"></param>
        public static void Delete(string strName)
        {
            RegisterHttpContextExtensions.Current.Response.Cookies.Delete(strName);
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        public static string DominName
        {
            get
            {
                return RegisterHttpContextExtensions.Current.Request.Host.Host;
            }
        }

        /// <summary>
        /// 获取端口
        /// </summary>
        public static int Port
        {
            get
            {
                return RegisterHttpContextExtensions.Current.Request.Host.Port.ToInt();
            }
        }
    }
}
