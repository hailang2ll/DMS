using DMS.Common.Extensions;
using DMS.Extensions.Cookies;

namespace DMS.Authorizations.UserContext.Cookies
{
    public class UserAuth : IUserAuth
    {
        private readonly ICookieHelper _cookieHelper;
        public UserAuth(ICookieHelper cookieHelper)
        {
            this._cookieHelper = cookieHelper;
        }
        public long ID => _cookieHelper.GetCookie("ID").ToLong();
        public string Name => _cookieHelper.GetCookie("UserName");


    }
}
