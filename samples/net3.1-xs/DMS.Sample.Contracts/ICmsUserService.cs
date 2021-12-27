using DMS.Common.Model.Result;
using DMS.Sample.Contracts.Param;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts
{
    public interface ICmsUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ResponseResult> Login(LoginCmsUserParam param);
    }
}
