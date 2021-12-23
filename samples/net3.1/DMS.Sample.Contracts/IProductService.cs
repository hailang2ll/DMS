using DMS.Common.Model.Result;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<UserEntity>> GetProductAsync(long jobLogID);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task TestRedis0(string msg = "我应该在0库");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task TestRedis1(string msg = "我应该在1库");
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task RedisPublish();
        /// <summary>
        /// 
        /// </summary>
        void Publish();
        /// <summary>
        /// 
        /// </summary>
        void Subscribe();

    }
}
