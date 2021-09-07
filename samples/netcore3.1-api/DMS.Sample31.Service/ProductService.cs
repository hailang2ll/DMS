using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using System.Threading.Tasks;

namespace DMS.Sample31.Service
{
    public class ProductService : IProductService
    {
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long jobLogID)
        {
            ResponseResult<UserEntity> result = new ResponseResult<UserEntity>();
            UserEntity entity = new UserEntity()
            {
                UserID = 1125964271981826048,
                UserName = "aaaa",
                Pwd = "pwd"
            };
            result.data = entity;
            return await Task.FromResult(result);

        }
    }
}
