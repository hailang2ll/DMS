﻿using DMS.Extensions.DBExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class SqlsugarSetup
    {
        /// <summary>
        /// .NET单例注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration)
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
                //循环输出sql
                allCon.ForEach(q =>
                {
                    db.GetConnection(q.ConnId).Aop.OnLogExecuting = (sql, pars) =>
                    {
                        if (q.SqlPrint)
                        {
                            Console.WriteLine(sql);//输出sql
                            Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
                        }
                    };
                });

            });
            services.AddSingleton<ISqlSugarClient>(sqlSugar);
        }

        /// <summary>
        /// SqlSugar.IOC注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSqlsugarIocSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var listConfig = new List<IocConfig>();
            var allCon = DBConfig.MutiInitConn();
            allCon.ForEach(q =>
            {
                listConfig.Add(new IocConfig()
                {
                    DbType = (IocDbType)q.DbType,
                    ConnectionString = q.Connection,
                    IsAutoCloseConnection = true,
                    ConfigId = q.ConnId,
                });
            });

            SugarIocServices.AddSqlSugar(listConfig);
            SugarIocServices.ConfigurationSugar(db =>
            {
                allCon.ForEach(q =>
                {
                    db.GetConnection(q.ConnId).Aop.OnLogExecuting = (sql, p) =>
                    {
                        if (q.SqlPrint)
                        {
                            Console.WriteLine(sql);//输出sql
                            Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                        }
                    };
                });

            });

        }
    }
}
