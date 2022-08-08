using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;

namespace DMS.Swagger
{
    public enum AuthType
    {
        Outh20,
        Simple,

    }
    public static class SwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenSetup(this IServiceCollection services, Action<SwaggerOption> swaggerOption, AuthModel authModel = AuthModel.Jwt)
        {
            // 添加Swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Demo",
                    Version = "v1",
                    Description = "API文档描述",
                    Contact = new OpenApiContact
                    {
                        Email = "79522860@qq.com",
                        Name = "dylan",
                        Url = new Uri("https://github.com/hailang2ll")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证名称",
                        Url = new Uri("https://github.com/hailang2ll")
                    }

                });
                // 获取xml文件名
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // 获取xml文件路径
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                #region 加载实体注释
                SwaggerOption swaggerConfig = new SwaggerOption();
                swaggerOption?.Invoke(swaggerConfig);
                if (swaggerConfig == null)
                {
                    ConsoleHelper.WriteErrorLine($"SwaggerGen xml load fail;swaggerOption is null");
                }
                else
                {
                    if (swaggerConfig.RootPath.IsNullOrEmpty() || swaggerConfig.XmlFiles.IsNullOrEmpty())
                    {
                        ConsoleHelper.WriteErrorLine($"SwaggerGen xml load fail;swaggerOption.RootPath or XmlFiles is null");
                    }
                    else
                    {
                        var basePath = swaggerConfig.RootPath; //AppContext.BaseDirectory;
                        foreach (var item in swaggerConfig.XmlFiles)
                        {
                            var xmlPath = Path.Combine(basePath, item);
                            if (File.Exists(xmlPath))
                            {
                                option.IncludeXmlComments(xmlPath, true);
                                ConsoleHelper.WriteSuccessLine($"SwaggerGen.contracts load success:path={xmlPath}");
                            }
                            else
                            {
                                ConsoleHelper.WriteErrorLine($"SwaggerGen xml load fail;path={xmlPath}");
                            }
                        }
                    }

                }
                #endregion

                void Token()
                {
                    option.OperationFilter<AddRequiredHeaderParameter>("AccessToken");
                }
                void Auth2()
                {
                    option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT认证授权，使用直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",  //jwt 默认参数名称
                        In = ParameterLocation.Header,  //jwt默认存放Authorization信息的位置（请求头）
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    //开启加权小锁
                    option.OperationFilter<AddResponseHeadersFilter>();
                    option.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                    //在header中添加token，传递到后台
                    option.OperationFilter<SecurityRequirementsOperationFilter>();
                }
                if (authModel == AuthModel.Cookies)
                {
                    ConsoleHelper.WriteInfoLine($"当前swagger.cookies认证");
                }
                else if (authModel == AuthModel.Token)
                {
                    Token();
                    ConsoleHelper.WriteInfoLine($"当前swagger.token认证,需开启redis");
                }
                else if (authModel == AuthModel.Jwt)
                {
                    Auth2();
                    ConsoleHelper.WriteInfoLine($"当前swagger.auth2认证,需开启jwt+redis");
                }
                else if (authModel == AuthModel.Id4)
                {
                    ConsoleHelper.WriteInfoLine($"当前swagger.id4认证,还未实现功能");
                }
                else
                {
                    ConsoleHelper.WriteInfoLine($"未知认证方式");
                }
            });


            return services;
        }


        public class AddRequiredHeaderParameter : IOperationFilter
        {
            private string _tenantIdExample;

            public AddRequiredHeaderParameter(string tenantIdExample)
            {
                if (string.IsNullOrEmpty(tenantIdExample))
                    throw new ArgumentNullException(nameof(tenantIdExample));

                _tenantIdExample = tenantIdExample;
            }

            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {

                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "AccessToken",
                    Description = "访问令牌",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "String" },
                    Required = false,
                    Example = new OpenApiString(_tenantIdExample)
                });
            }
        }
    }
}
