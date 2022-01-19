using DMS.Common.Model.Result;
using System.Threading.Tasks;

namespace DMS.Auth.Oauth2
{
    public interface IUserAuth
    {
        string Name { get; }
        long ID { get; }
        (bool, ResponseResult) ChenkLogin();
        (bool, ResponseResult<T>) ChenkLogin<T>();
        Task<(bool, ResponseResult)> ChenkLoginAsync();
        Task<(bool, ResponseResult<T>)> ChenkLoginAsync<T>();
        string GetToken();
        bool IsAuthenticated();
        long ID2 { get; }
        string Name2 { get; }
    }
}
