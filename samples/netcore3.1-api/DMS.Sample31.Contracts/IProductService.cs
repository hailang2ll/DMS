using DMSN.Common.BaseResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Sample31.Contracts
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
        void Publish();
        /// <summary>
        /// 
        /// </summary>
        void Subscribe();

    }
}
