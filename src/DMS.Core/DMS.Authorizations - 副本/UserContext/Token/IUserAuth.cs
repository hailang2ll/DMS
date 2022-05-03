using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.UserContext.Token
{
    public interface IUserAuth
    {
        long ID { get; }
        string EpCode { get; }
        string UID { get; }
        string Name { get; }
        string GetToken();
        UserTicket UserTicket { get; }
    }
}
