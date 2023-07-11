﻿using DMS.Common.Model.Param;
using DMS.Common.Model.Result;
using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DMS.Repository
{
    /// <summary>
    /// 基础仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<TEntity> : SimpleClient<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        public ITenant itenant = null;//多租户事务
        public BaseRepository(ISqlSugarClient context = null) : base(context)
        {
            itenant = DbScoped.SugarScope;//设置租户接口,事物用
            base.Context = DbScoped.SugarScope.GetConnectionScopeWithAttr<TEntity>();
        }

        #region 查询实体,select()用法
        public async Task<TResult> GetEntity<TResult>(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Queryable<TEntity>().Where(predicate).Select<TResult>().FirstAsync();
        }
        public async Task<TResult> GetEntity<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Queryable<TEntity>().Where(predicate).Select(expression).FirstAsync();
        }
        #endregion


        #region 查询列表，返回TEntity用法
        public async Task<List<TEntity>> QueryList(string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>()
                .OrderByIF(strOrderByFileds != null, strOrderByFileds)
                .ToListAsync();
        }
        public async Task<List<TEntity>> QueryList(Expression<Func<TEntity, object>> orderByExpression)
        {
            return await Context.Queryable<TEntity>()
                .OrderByIF(orderByExpression != null, orderByExpression)
                .ToListAsync();
        }
        public async Task<List<TEntity>> QueryList(Expression<Func<TEntity, bool>> predicate, string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(strOrderByFileds != null, strOrderByFileds)
                .ToListAsync();
        }
        public async Task<List<TEntity>> QueryList(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression)
                .ToListAsync();
        }
        public async Task<List<TEntity>> QueryList(int top, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression)
                .Take(top)
                .ToListAsync();
        }
        public async Task<List<TEntity>> QueryList(Expression<Func<TEntity, TEntity>> selectExpression, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression)
                .Select(selectExpression)
                .ToListAsync();
        }
        public async Task<List<TEntity>> QueryList(int top, Expression<Func<TEntity, TEntity>> selectExpression, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression)
                .Select(selectExpression)
                .Take(top)
                .ToListAsync();
        }

        #endregion
        #region 查询列表，返回TResult用法
        public async Task<List<TResult>> QueryRList<TResult>(string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>()
                .OrderBy(strOrderByFileds)
                .Select<TResult>()
                .ToListAsync();
        }
        public async Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, object>> orderByExpression)
        {
            return await Context.Queryable<TEntity>()
                .OrderByIF(orderByExpression != null, orderByExpression)
                .Select<TResult>()
                .ToListAsync();
        }
        public async Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, bool>> predicate, string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(strOrderByFileds != null, strOrderByFileds)
                .Select<TResult>()
                .ToListAsync();
        }
        public async Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression)
                .Select<TResult>()
                .ToListAsync();
        }
        public async Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            return await Context.Queryable<TEntity>()
                .WhereIF(predicate != null, predicate)
                .OrderByIF(orderByExpression != null, orderByExpression)
                .Select(selectExpression)
                .ToListAsync();
        }

        #endregion

        #region 分页列表，返回TEntity用法
        public async Task<PageModel<TEntity>> QueryPageList(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, string strOrderByFileds)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TEntity>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }
        public async Task<PageModel<TEntity>> QueryPageList(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(orderByExpression != null, orderByExpression)
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TEntity>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }
        public async Task<PageModel<TEntity>> QueryPageList(Expression<Func<TEntity, TEntity>> selectExpression, Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(orderByExpression != null, orderByExpression)
             .Select(selectExpression)
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TEntity>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }
        #endregion
        #region 分页列表，返回TResult用法
        public async Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, string strOrderByFileds)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .Select<TResult>()
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TResult>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }
        public async Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(orderByExpression != null, orderByExpression)
             .Select<TResult>()
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TResult>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }

        public async Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>> predicate, PageParam pageParam, string strOrderByFileds)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .Select(selectExpression)
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TResult>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }
        public async Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await Context.Queryable<TEntity>()
             .WhereIF(predicate != null, predicate)
             .OrderByIF(orderByExpression != null, orderByExpression)
             .Select(selectExpression)
             .ToPageListAsync(pageParam.pageIndex, pageParam.pageSize, totalCount);

            return new PageModel<TResult>() { pageIndex = pageParam.pageIndex, pageSize = pageParam.pageSize, totalRecord = totalCount, resultList = list };
        }

        #endregion

        #region 事物委托
        /// <summary>
        /// 多租户异常事物
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<DbResult<bool>> UseITenantTran(Func<Task> action)
        {
            var resultTran = await itenant.UseTranAsync(async () =>
            {
                await action();
            });
            return resultTran;

        }
        /// <summary>
        /// 同一对句事物处理
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<DbResult<bool>> UseTran(Func<Task> action)
        {
            var resultTran = await Context.Ado.UseTranAsync(async () =>
            {
                await action();
            });
            return resultTran;
        }
        #endregion
    }
}
