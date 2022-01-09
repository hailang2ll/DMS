using DMS.Common.Model.Result;
using DMS.Sample.Contracts.Param;
using DMS.Sample.Contracts.Result;
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
        Task<ResponseResult<LoginCmsUserResult>> Login(LoginCmsUserParam param);
    }
}
