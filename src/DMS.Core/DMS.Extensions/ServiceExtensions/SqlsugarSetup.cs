using DMS.Extensions.DBExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            var listConfig = new List<ConnectionConfig>();
            var allCon = DBConfig.MutiInitConn();
            allCon.ForEach(q =>
            {
                listConfig.Add(new ConnectionConfig()
                {
                    DbType = (DbType)q.DbType,
                    ConnectionString = q.Connection,
                    IsAutoCloseConnection = true,
                    ConfigId = q.ConnId,
                });
            });
            SqlSugarScope sqlSugar = new SqlSugarScope(listConfig,
            db =>
            {
                allCon.ForEach(q =>
                {
                    db.GetConnection(q.ConnId).Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine(sql);//输出sql
                        Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
                    };
                });

            });
            services.AddSingleton<ISqlSugarClient>(sqlSugar);
        }
    }
}
