using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Cookies
{
    public interface ICookieHelper
    {
        void SetCookie(string key, string value);

        void SetCookie(string key, string value, int expiresTime);

        string GetCookie(string key);

        void DeleteCookie(string key);
    }
}
