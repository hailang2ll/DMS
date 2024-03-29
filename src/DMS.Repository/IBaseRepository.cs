﻿using DMS.Common.Model.Param;
using DMS.Common.Model.Result;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        #region 查询实体,select()用法
        Task<TResult> GetEntity<TResult>(Expression<Func<TEntity, bool>> predicate);
        Task<TResult> GetEntity<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 查询列表，返回TEntity用法
        Task<List<TEntity>> QueryList(string strOrderByFileds);
        Task<List<TEntity>> QueryList(Expression<Func<TEntity, object>> orderByExpression);
        Task<List<TEntity>> QueryList(Expression<Func<TEntity, bool>> predicate, string strOrderByFileds);
        Task<List<TEntity>> QueryList(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null);
        Task<List<TEntity>> QueryList(int top, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null);
        Task<List<TEntity>> QueryList(Expression<Func<TEntity, TEntity>> selectExpression, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null);
        Task<List<TEntity>> QueryList(int top, Expression<Func<TEntity, TEntity>> selectExpression, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null);
        #endregion
        #region 查询列表，返回TResult用法
        Task<List<TResult>> QueryRList<TResult>(string strOrderByFileds);
        Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, object>> orderByExpression);
        Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, bool>> predicate, string strOrderByFileds);
        Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null);
        Task<List<TResult>> QueryRList<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderByExpression = null);
        #endregion

        #region 分页列表，返回TEntity用法
        Task<PageModel<TEntity>> QueryPageList(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, string strOrderByFileds);
        Task<PageModel<TEntity>> QueryPageList(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null);
        Task<PageModel<TEntity>> QueryPageList(Expression<Func<TEntity, TEntity>> selectExpression, Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null);
        #endregion
        #region 分页列表，返回TResult用法
        Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, string strOrderByFileds);
        Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null);
        Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>> predicate, PageParam pageParam, string strOrderByFileds);
        Task<PageModel<TResult>> QueryPageList<TResult>(Expression<Func<TEntity, TResult>> selectExpression, Expression<Func<TEntity, bool>> predicate, PageParam pageParam, Expression<Func<TEntity, object>> orderByExpression = null);
        #endregion
       

        #region 事物委托
        Task<DbResult<bool>> UseITenantTran(Func<Task> action);
        Task<DbResult<bool>> UseTran(Func<Task> action);
        #endregion
    }
}
