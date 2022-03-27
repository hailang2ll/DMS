using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Param;
using DMS.IServices.Result;
using DMS.Models;
using SqlSugar;
using System.Linq.Expressions;

namespace DMS.Services
{
    public class YxyMemberService : IYxyMemberService
    {
        public ISqlSugarClient _mainDB;
        public YxyMemberService(ISqlSugarClient sqlSugar)
        {
            //默认为主对象
            _mainDB = sqlSugar;
        }
        //子对象
        public SqlSugarProvider _slaveDB => _mainDB.AsTenant().GetConnection("yxy_system");

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="jobLogID"></param>
        /// <returns></returns>
        public async Task<ResponseResult<YxyMemberResult>> GetMemberAsync(long memberID)
        {
            ResponseResult<YxyMemberResult> result = new() { data = new YxyMemberResult() };
            var entity = await _slaveDB.Queryable<YxyMember>()
                .Select<YxyMemberResult>()
                .FirstAsync(q => q.Id > 0);
            if (entity == null)
            {
                result.errno = 1;
                result.errmsg = "未找到相关数据";
                return result;
            }
            result.data = entity;
            return result;
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="jobLogType"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<YxyMemberResult>>> GetMemberListAsync(long memberType)
        {
            ResponseResult<List<YxyMemberResult>> result = new()
            {
                data = new List<YxyMemberResult>()
            };
            if (memberType <= 0)
            {
                result.errno = 1;
                result.errmsg = "参数不合法";
                return result;
            }
            var list = await _slaveDB.Queryable<YxyMember>()
                .Where(q => q.Id > 0)
                .Select<YxyMemberResult>()
                .ToListAsync();
            if (list == null || list.Count <= 0)
            {
                result.errno = 2;
                result.errmsg = "未找到相关数据";
                return result;
            }
            result.data = list;
            return result;
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseResult<PageModel<YxyMemberResult>>> SearchMemberAsync(SearchYxyMemberParam param)
        {
            ResponseResult<PageModel<YxyMemberResult>> result = new()
            {
                data = new PageModel<YxyMemberResult>()
            };
            if (param == null)
            {
                result.errno = 1;
                result.errmsg = "参数不合法";
                return result;
            }

            RefAsync<int> totalCount = 0;
            var expression = Expressionable.Create<YxyMember>();
            expression.And(m => m.Id == 1);
            Expression<Func<YxyMember, bool>> where = expression.ToExpression();
            var list = await _slaveDB.Queryable<YxyMember>()
                .WhereIF(where != null, where)
                .OrderBy(q => q.Id, OrderByType.Desc)
                .Select<YxyMemberResult>()
                .ToPageListAsync(param.pageIndex, param.pageSize, totalCount);
            if (list == null || list.Count <= 0)
            {
                result.errno = 2;
                result.errmsg = "未找到相关数据";
                return result;
            }
            result.data.resultList = list;
            result.data.pageIndex = param.pageIndex;
            result.data.pageSize = param.pageSize;
            result.data.totalRecord = (int)totalCount;
            return result;
        }
    }
}
