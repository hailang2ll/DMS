using Autofac;
using Autofac.Extensions.DependencyInjection;
using DMS.Api.Filter;
using DMS.Authorizations.Model;
using DMS.Authorizations.ServiceExtensions;
using DMS.Common.Extensions;
using DMS.Common.JsonHandler.JsonConverters;
using DMS.Common.Model.Result;
using DMS.Extensions.ServiceExtensions;
using DMS.NLogs;
using DMS.NLogs.Filters;
using DMS.Redis.Configurations;
using DMS.Services.RedisEvBus;
using DMS.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCommandLine(args);
builder.WebHost.UseUrls(builder.Configuration.GetValue<string>("StartUrl"));
builder.Host
.ConfigureLogging((hostingContext, builder) =>
{
    //builder.AddFilter("System", LogLevel.Error);
    //builder.AddFilter("Microsoft", LogLevel.Error);
    //builder.SetMinimumLevel(LogLevel.Error);
    builder.UseNLog(Path.Combine(Directory.GetCurrentDirectory(), "Configs/nlog.config"));
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
    config.AddCommandLine(args);
    var env = hostingContext.HostingEnvironment;
    config.AddJsonFile($"Configs/redis.json", optional: false, reloadOnChange: true);
    //config.AddJsonFile($"Configs/domain.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);
    config.AddAppSettingsFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
});


builder.Services.AddControllers(option =>
{
    //ȫ�ִ����쳣��֧��DMS.Log4net��DMS.NLogs
    option.Filters.Add<GlobalExceptionFilter>();

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    //options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //options.JsonSerializerOptions.DictionaryKeyPolicy = null;
})
.AddFluentValidation(config =>
{
    //���򼯷�ʽ�����֤
    //config.RegisterValidatorsFromAssemblyContaining(typeof(AddMemberParamValidator));

    //���������֤
    var validatorList = DMS.Common.Extensions.TypeExtensions.GetTypeList("DMS.IServices", "Validator");
    foreach (var item in validatorList)
    {
        config.RegisterValidatorsFromAssemblyContaining(item);
    }
})
.ConfigureApiBehaviorOptions(options =>
{
    //ʹ���Զ���ģ����֤
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
//api�ĵ����ɣ�1֧����ͨtoken��֤��2֧��oauth2�л���Ĭ��Ϊ1
builder.Services.AddSwaggerGenSetup(option =>
{
    option.RootPath = AppContext.BaseDirectory;
    option.XmlFiles = new List<string> {
        AppDomain.CurrentDomain.FriendlyName+".xml",
        "DMS.IServices.xml"
    };
});
//����HttpContext����
builder.Services.AddHttpContextSetup();
//����sqlsugar����
builder.Services.AddSqlsugarIocSetup(builder.Configuration);
//����redis����
builder.Services.AddRedisSetup();
//����redismq����
builder.Services.AddRedisMqSetup();
//���������֤������api�ĵ���֤��Ӧ���ɣ�Ҫ�ȿ���redis����
builder.Services.AddUserContextSetup();

Permissions.IsUseIds4 = DMS.Common.AppConfig.GetValue(new string[] { "IdentityServer4", "Enabled" }).ToBool();
builder.Services.AddAuthorizationSetup();
// ��Ȩ+��֤ (jwt or ids4)
if (Permissions.IsUseIds4)
{
    builder.Services.AddAuthenticationIds4Setup();
}
else
{
    builder.Services.AddAuthenticationJWTSetup();
}
//�����������
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
// CORS����
app.UseCors(DMS.Common.AppConfig.GetValue(new string[] { "Cors", "PolicyName" }));
//������̬ҳ��
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
