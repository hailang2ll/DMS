using DMS.IServices;
using SqlSugar;

namespace DMS.Services
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
