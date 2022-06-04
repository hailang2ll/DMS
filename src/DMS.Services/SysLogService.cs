using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Param;
using DMS.Models;
using DMS.Repository;

namespace DMS.Services
{
    /// <summary>
    /// .net ioc注入
    /// </summary>
    public class SysLogService : BaseService<SysLog>, ISysLogService
    {
        public async Task<int> Add0(AddSysLogParam param)
        {
            SysLog log = new()
            {
                MemberName = param.MemberName,
                SubSysid = param.SubSysid,
                SubSysname = param.SubSysname,
                Ip = "",
                Url = "",
                Thread = "",
                Level = "",
                Logger = "",
                Message = "",
                LogType = 1,
                Exception = "",
                CreateTime = DateTime.Now,
            };
            var flag = await base.AsInsertable(log).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
            return flag;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Add(AddSysLogParam param)
        {
            ResponseResult result = new();
            SysLog log = new()
            {
                MemberName = param.MemberName,
                SubSysid = param.SubSysid,
                SubSysname = param.SubSysname,
                Ip = "",
                Url = "",
                Thread = "",
                Level = "",
                Logger = "",
                Message = "",
                LogType = 1,
                Exception = "",
                CreateTime = DateTime.Now,
            };

            //返回true,false
            var bflag = await base.InsertAsync(log);
            //插入主键id
            var lflag = await base.InsertReturnBigIdentityAsync(log);
            var flag = await base.InsertReturnIdentityAsync(log);

            //null 列不插入
            flag = await base.AsInsertable(log).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
            //插入指定列
            flag = await base.AsInsertable(log).InsertColumns(it => new { it.MemberName, it.SubSysid }).ExecuteReturnIdentityAsync();
            flag = await base.AsInsertable(log).InsertColumns("Name", "JobLogType").ExecuteReturnIdentityAsync();
            result.data = flag;
            return result;
        }

        /// <summary>
        /// 事物处理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseResult> AddTran(AddSysLogParam param)
        {
            ResponseResult result = new();
            try
            {
                itenant.BeginTran();
                SysLog log = new()
                {
                    MemberName = param.MemberName,
                    SubSysid = param.SubSysid,
                    SubSysname = param.SubSysname,
                    Ip = "",
                    Url = "",
                    Thread = "",
                    Level = "",
                    Logger = "",
                    Message = "",
                    LogType = 1,
                    Exception = "",
                    CreateTime = DateTime.Now,
                };
                await base.InsertAsync(log);

                //同库
                SysJoblog joblog = new()
                {
                    Name = param.MemberName,
                    JobLogtype = 1,
                    ServerIp = IPHelper.GetCurrentIp(),
                    TaskLogtype = 1,
                    Message = "a",
                    CreateTime = DateTime.Now,
                };
                var jobDal = base.ChangeRepository<BaseRepository<SysJoblog>>();//切换仓储
                await jobDal.InsertAsync(joblog);

                itenant.CommitTran();
            }
            catch (Exception ex)
            {
                result.errmsg = ex.Message;
                itenant.RollbackTran();
            }
            return result;
        }

    }
}
