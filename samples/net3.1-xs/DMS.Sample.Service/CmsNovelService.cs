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
    public class CmsNovelService : ICmsNovelService
    {
        public ISqlSugarClient db;
        public CmsNovelService(ISqlSugarClient sqlSugar)
        {
            db = sqlSugar;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Add(AddCmsNovelParam param)
        {
            ResponseResult result = new ResponseResult();
            if (param == null
               || string.IsNullOrEmpty(param.Title)
               || string.IsNullOrEmpty(param.Content))
            {
                result.errno = 1;
                result.errmsg = "参数错误";
                return result;
            }

            CmsNovel cmsNovel = new CmsNovel()
            {
                Title = param.Title,
            };
            var flag = await db.Insertable(cmsNovel).ExecuteReturnIdentityAsync();
            if (flag > 0)
            {
                result.errmsg = "添加成功";
            }
            else
            {
                result.errno = 2;
                result.errmsg = "添加失败";
            }
            return result;
        }
    }
}
