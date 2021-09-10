using DMSN.Common.BaseResult;
using System.Threading.Tasks;

namespace DMS.Auth
{
    public interface IUserAuth
    {
        string Name { get; }
        long ID { get; }
        string GetToken();
        (bool, ResponseResult) ChenkLogin();
        (bool, ResponseResult<T>) ChenkLogin<T>();
        Task<(bool, ResponseResult)> ChenkLoginAsync();
        Task<(bool, ResponseResult<T>)> ChenkLoginAsync<T>();
    }
}
