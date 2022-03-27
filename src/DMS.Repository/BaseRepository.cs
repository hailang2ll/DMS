using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Repository
{
    /// <summary>
    /// 仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : SimpleClient<T> where T : class, new()
    {
        public ITenant itenant = null;//多租户事务
        public BaseRepository(ISqlSugarClient context = null) : base(context)
        {
            //通过特性拿到ConfigId
            var configId = typeof(T).GetCustomAttribute<TenantAttribute>()?.configId;
            if (configId != null)
            {
                Context = DbScoped.SugarScope.GetConnection(configId);
                itenant = DbScoped.SugarScope;//设置租户接口
            }
        }

        /// <summary>
        /// 扩展方法，自带方法不能满足的时候可以添加新方法
        /// </summary>
        /// <returns></returns>
        public List<T> CommQuery(string json)
        {
           return base.Context.Queryable<T>().ToList();
        }
    }
}
