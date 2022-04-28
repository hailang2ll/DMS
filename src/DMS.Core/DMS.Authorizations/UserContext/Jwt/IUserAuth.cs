using DMS.Authorizations.Policys;
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
        string GetToken();
        bool IsAuthenticated();
    }
}
