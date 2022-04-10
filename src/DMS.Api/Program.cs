using Autofac;
using Autofac.Extensions.DependencyInjection;
using DMS.Api.Filter;
using DMS.Auth;
using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Common.JsonHandler.JsonConverters;
using DMS.Common.Model.Result;
using DMS.Extensions.Authorizations.Model;
using DMS.Extensions.ServiceExtensions;
using DMS.NLogs;
using DMS.NLogs.Filters;
using DMS.Redis.Configurations;
using DMS.Services.RedisEvBus;
using DMS.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host
.ConfigureLogging((hostingContext, builder) =>
{
    //builder.AddFilter("System", LogLevel.Error);
    //builder.AddFilter("Microsoft", LogLevel.Error);
    //builder.SetMinimumLevel(LogLevel.Error);
    //builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
})
.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModuleRegister(AppContext.BaseDirectory, new List<string>()
    {
        "DMS.Services.dll",
    }));
    builder.RegisterModule<AutofacPropertityModuleRegister>();
})
.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.Sources.Clear();
    config.AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);
    config.AddAppSettingsFile($"appsettings.json", optional: true, reloadOnChange: true);
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
        var result = new ResponseResult()
        {
            errno = 1,
            errmsg = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
        };
        return new JsonResult(result);
    };
});
//api文档生成，1支持普通token验证，2支持oauth2切换；默认为1
builder.Services.AddSwaggerGenSetup(option =>
{
    option.RootPath = AppContext.BaseDirectory;
    option.XmlFiles = new List<string> {
        AppDomain.CurrentDomain.FriendlyName+".xml",
        "DMS.IServices.xml"
    };
});
//开启HttpContext服务
builder.Services.AddHttpContextSetup();
//开启sqlsugar服务
builder.Services.AddSqlsugarIocSetup(builder.Configuration);
//开启redis服务
builder.Services.AddRedisSetup();
//开启redismq服务
builder.Services.AddRedisMqSetup();
//开启身份认证服务，与api文档验证对应即可，要先开启redis服务
builder.Services.AddAuthSetup();

Permissions.IsUseIds4 = DMS.Common.AppConfig.GetValue(new string[] { "IdentityServer4", "Enabled" }).ToBool();
builder.Services.AddAuthorizationSetup();
// 授权+认证 (jwt or ids4)
if (Permissions.IsUseIds4)
{
    builder.Services.AddAuthenticationIds4Setup();
}
else
{
    builder.Services.AddAuthenticationJWTSetup();
}
//开启跨域服务
//services.AddCorsSetup();
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseSwaggerUI(true);
// CORS跨域
app.UseCors(DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" }));
//开户静态页面
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
