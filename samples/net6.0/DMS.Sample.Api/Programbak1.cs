using Autofac;
using Autofac.Extensions.DependencyInjection;
using DMS.Auth;
using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Common.JsonHandler.JsonConverters;
using DMS.Common.Model.Result;
using DMS.Extensions;
using DMS.Extensions.ServiceExtensions;
using DMS.NLogs;
using DMS.NLogs.Filters;
using DMS.Redis.Configurations;
using DMS.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YXY.Member.Api.Filter;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureDefaults(args).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseUrls("http://*:20300");
    webBuilder.UseNLog($"Configs/nlog.config");
    webBuilder.UseStartup<Startup>()
});

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
    .AddAppSettingsFile($"appsettings{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();


#region autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModuleRegister(AppContext.BaseDirectory, new List<string>()
            {
                "DMS.Sample.Service.dll",
            }));
    builder.RegisterModule<AutofacPropertityModuleRegister>();
});
#endregion
builder.Host.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseUrls("http://*:20300");
    webBuilder.UseNLog($"Configs/nlog.config");

    webBuilder.ConfigureAppConfiguration((hostContext, config) =>
    {
        var env = hostContext.HostingEnvironment;
        config.SetBasePath(env.ContentRootPath)
            .AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddAppSettingsFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    });
});



builder.Services.AddControllers(option =>
{
    //全局处理异常，支持DMS.Log4net，DMS.NLogs
    option.Filters.Add<GlobalExceptionFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
    //options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //options.JsonSerializerOptions.DictionaryKeyPolicy = null;
}).ConfigureApiBehaviorOptions(options =>
{
    //使用自定义模型验证
    options.InvalidModelStateResponseFactory = (context) =>
    {
        var result = new ResponseResult();
        result.errmsg = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        return new JsonResult(result);
    };
});

//api文档生成，1支持普通token验证，2支持oauth2切换；默认为1
builder.Services.AddSwaggerGenSetup(AuthModel.All);
////开启redis服务
builder.Services.AddRedisSetup();
//开启HttpContext服务
builder.Services.AddHttpContextSetup();
//开启身份认证服务，与api文档验证对应即可
builder.Services.AddAuthSetup(AuthModel.All);
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
};
app.UseSwaggerUI();
// CORS跨域
app.UseCors(DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" }));
//开户静态页面
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


