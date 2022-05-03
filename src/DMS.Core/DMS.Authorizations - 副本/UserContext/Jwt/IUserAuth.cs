using DMS.Authorizations.Policys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Authorizations.UserContext.Jwt
{
    public interface IUserAuth
    {
        long Uid { get; }
        long Cid { get; }
        string EpCode { get; }
        Task<List<PermissionItem>> GetPermissionDatas();
        Task<bool> SetTokenExpireAsync(string token, DateTime exprise);
        string GetToken();
        bool IsAuthenticated();
    }
}
