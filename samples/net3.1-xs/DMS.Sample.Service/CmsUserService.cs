using DMS.Common.Extensions;
using DMS.Common.Model.Result;
using DMS.Sample.Contracts;
using DMS.Sample.Contracts.Param;
using DMS.Sample.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Sample.Service
{
    public class CmsUserService : ICmsUserService
    {
        public ISqlSugarClient db;
        public CmsUserService(ISqlSugarClient sqlSugar)
        {
            db = sqlSugar;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Login(LoginCmsUserParam param)
        {
            ResponseResult result = new ResponseResult();
            //if (param.UserName.IsNullOrEmpty()
            //    || param.UserPassword.IsNullOrEmpty())
            //{
            //    result.errno = 1;
            //    result.errmsg = "参数错误";
            //    return result;
            //}

            var entity = await db.Queryable<CmsUser>()
             .FirstAsync(q => q.UserName == param.UserName && q.UserPassword == param.UserPassword);
            if (entity == null)
            {
                result.errno = 2;
                result.errmsg = "未找到相关数据";
                return result;
            }

            result.data = entity;
            return result;
        }
    }
}
