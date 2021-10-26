using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Auth.v1
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
