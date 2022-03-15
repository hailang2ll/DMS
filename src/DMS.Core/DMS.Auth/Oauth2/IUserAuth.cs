using DMS.Common.Model.Result;
using System.Threading.Tasks;

namespace DMS.Auth.Oauth2
{
    public interface IUserAuth
    {
        long ID { get; }
        string Name { get; }
        string EpCode { get; }

        string GetToken();
        bool IsAuthenticated();
    }
}
