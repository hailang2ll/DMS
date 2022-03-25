using DMS.Sample.Contracts;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DMS.Sample.Service
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        public ISqlSugarClient _db;
        public BaseServices(ISqlSugarClient sqlSugar)
        {
            _db = sqlSugar;
        }

        public async Task<TEntity> QueryById(object objId)
        {
            return await _db.Queryable<TEntity>().InSingleAsync(objId);
           
        }
    }

}
