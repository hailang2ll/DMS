using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.ServiceExtensions
{
    public static class FreesqlSetup
    {
        public static void AddFreesqlSetup(this IServiceCollection services, IConfiguration configuration, string dbName = "db_master")
        {
            IFreeSql Fsql = new FreeSql.FreeSqlBuilder()
               //.UseConnectionString(FreeSql.DataType.SqlServer, @"Data Source=192.168.31.201;User Id=devuser;Password=yxw-88888;Initial Catalog=trydou_sys;Pooling=true;Min Pool Size=1")
               .UseConnectionString(FreeSql.DataType.MySql, configuration.GetConnectionString(dbName))
              //.UseAutoSyncStructure(true)
              .Build();

            Fsql.Aop.CurdAfter += (s, e) =>
            {
                Console.WriteLine(e.Sql);
                if (e.ElapsedMilliseconds > 200)
                {
                    //记录日志
                    //发送短信给负责人
                    Console.WriteLine("超过了200毫秒");
                }
            };

            services.AddSingleton<IFreeSql>(Fsql);
        }
    }
}
