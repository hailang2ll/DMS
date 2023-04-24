using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.UserContext.Cookies
{
    public interface IUserAuth
    {
        long ID { get; }
        string Name { get; }
    }
}
