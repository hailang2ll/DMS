using DMS.Auth;
using DMS.Common.BaseResult;
using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Common.Serialization;
using DMS.Sample31.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DMS.Sample31.Controllers
{
    /// <summary>
    /// Common api 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public IUserService _userService { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public IProductService _productservice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productservice"></param>
        public CommonController(IProductService productservice)
        {
            _productservice = productservice;
        }

        /// <summary>
        /// 测试IOC，测试token验证，测试属性认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetShopLogo")]
        public Task<ResponseResult<UserEntity>> GetShopLogo([FromQuery]UserEntity param)
        {
            var (loginFlag, result) = ChenkLogin<UserEntity>(UserTicket);
            if (!loginFlag)
            {
                return Task.FromResult(result);
            }

            var b = _productservice.Add();
            //var i = _userService.Add();//未构造，验证属性注入

            UserEntity user = new UserEntity()
            {
                UserID = 1,
                UserName = "hailang",
                Pwd = "123456",
                Time = DateTime.Now,
            };
            result.data = user;
            return Task.FromResult(result);
        }

        #region GetLog4net
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLog4net")]
        public ActionResult GetLog4net()
        {
            DMS.Log4net.Logger.Info("这是log4net的日志");
            DMS.Log4net.Logger.Error("这是log4net的异常日志");

            DMS.NLogs.Logger.Debug("这是nlog的Debug日志");
            DMS.NLogs.Logger.Info("这是nlog的日志");
            DMS.NLogs.Logger.Error("这是nlog的异常日志");
            var result = new
            {
                data = "成功"
            };
            return Ok(result);
        }
        #endregion

        /// <summary>
        /// 读取appsettings.json和自定义配置文件
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAppConfig")]
        public ActionResult GetAppConfig()
        {
            string memberApi = DMS.Common.AppConfig.GetVaule<string>("MemberUrl");
            memberApi = DMS.Common.AppConfig.GetVaule("MemberUrl");
            var ip = $"获取IP：{IPHelper.GetWebClientIp()}";
            var dev = DMS.Common.AppConfig.GetVaule("dev");
            var redisOption = DMS.Redis.AppConfig.RedisOption;


            var result = new
            {
                memberApi,
                ip,
                dev,
                redisOption
            };
            return Ok(result);
        }

        #region 自定义属性转换为实体
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("AttributeExtension")]
        public ActionResult AttributeExtension()
        {
            //if (_rabbitMqAttribute.IsNullOrEmpty())
            //{
            //    var typeOfT = typeof(MessageModel);
            //    _rabbitMqAttribute = typeOfT.GetCustomAttribute<RabbitMqAttribute>();
            //}


            MessageModel model = new MessageModel()
            {
                Msg = "aaaaaaaaaa",
                CreateDateTime = DateTime.Now,
            };
            Publish(model);
            return Content("");
        }

        private void Publish<T>(T command) where T : class
        {
            var queueInfo = GetRabbitMqAttribute<T>();
        }

        private static RabbitMqAttribute _rabbitMqAttribute;
        private static RabbitMqAttribute GetRabbitMqAttribute<T>()
        {
            if (_rabbitMqAttribute.IsNullOrEmpty())
            {
                var typeOfT = typeof(T);
                _rabbitMqAttribute = typeOfT.GetCustomAttribute<RabbitMqAttribute>();
            }
            return _rabbitMqAttribute;
        }
        #endregion

        #region EnumExtensions枚举扩展
        /// <summary>
        /// 枚举扩展
        /// </summary>
        /// <returns></returns>
        [HttpGet("EnumExtensions")]
        public ActionResult EnumExtensions()
        {
            var json = typeof(EnumMemUserType).ToJson();
            var des = EnumMemUserType.QQType.GetDescription();
            des = typeof(EnumMemUserType).GetDescription(2);
            return Ok(des);
        }
        #endregion

        #region SerializerExtensions序列化/反序列化
        /// <summary>
        /// 序列化/反序列化
        /// ConfigureServices=>AddJsonOptions{ options.SerializerSettings.Converters.Add(new CustomStringConverter());}
        /// [ColumnMapping(Name = "ShopMemberID"), JsonConverter(typeof(CustomStringConverter))]
        /// </summary>
        /// <returns></returns>
        [HttpGet("SerializerExtensions")]
        public ActionResult SerializerExtensions()
        {
            UserEntity user = new UserEntity()
            {
                UserID = 1125964271981826048,
                UserName = "aaaa",
                Pwd = "pwd"
            };
            string serObject = user.SerializeObject();
            UserEntity u = serObject.DeserializeObject<UserEntity>();

            string s = "aaaaaaaaaaa";
            var ss = s.SerializeObject();
            var bb = ss.DeserializeObject<string>();

            return Content("");
        }
        #endregion




        #region ExportBuilderHelper,ExportHelper导入导出excel
        #region ExportBuilderHelper,ExportHelper数据导出excel
        /// <summary>
        /// excel导出
        /// 请求方式：http://localhost:5000/api/Common/ExportFS
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportFS")]
        public FileResult ExportFS()
        {
            List<UserEntity> userList = new List<UserEntity>() {
                 new UserEntity(){ UserID=1, Pwd="aaaa", UserName="aaaaa" },
                 new UserEntity(){ UserID=2, Pwd="bbbb", UserName="bbbbb"},
            };
            var fs = ExportHelper.ExportToFsExcel<UserEntity>(userList, new string[] { "UserID", "UserName" },
               c => c.UserID,
               c => c.UserName.ToString()
               );

            return File(fs, "application/vnd.ms-excel", "ExportFS.xls");
        }

        /// <summary>
        /// excel导出
        /// 请求方式：http://localhost:5000/api/Common/ExportBuilderFS
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportBuilderFS")]
        public FileResult ExportBuilderFS()
        {
            List<UserEntity> userList = new List<UserEntity>() {
                  new UserEntity(){ UserID=1, Pwd="aaaa", UserName="aaaaa" },
                 new UserEntity(){ UserID=2, Pwd="bbbb", UserName="bbbbb"},
            };

            var fs = new ExportBuilderHelper<UserEntity>()
                   .Column(c => c.UserID)
                   .Column(c => c.UserName)
                   .Export(userList);

            return File(fs, "application/vnd.ms-excel", "ExportBuilderFS.xls");
        }

        /// <summary>
        /// API导出数据
        /// 请求方式：API接口，地址浏览
        /// </summary>
        /// <returns></returns>
        [HttpGet("Export")]
        public string Export()
        {
            List<UserEntity> list = new List<UserEntity>();
            list.Add(new UserEntity() { UserID = 1, UserName = "lang" });
            list.Add(new UserEntity() { UserID = 1, UserName = "aaa" });
            DataTable dt = list.ToDataTable("dd");
            return ExportHelper.ExportToExcel(dt, @"d:\Export.xls");
        }

        /// <summary>
        /// API导出数据
        /// 请求方式：API接口，地址浏览
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportToExcel")]
        public string ExportToExcel()
        {
            List<UserEntity> userList = new List<UserEntity>() {
                 new UserEntity(){ UserID=1,UserName="aaa" },
                 new UserEntity(){ UserID=2,UserName="bbb" },
            };
            var s = ExportHelper.ExportToExcel<UserEntity>(userList, @"d:\ExportToExcel.xls",
               new string[] { "用户ID", "用户名称" },
               c => c.UserID,
               c => c.UserName.ToString()
               );

            return s;
        }
        #endregion
        #region 导入excel
        /// <summary>
        /// 根据文件导入
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportToExcel")]
        public object ImportToExcel()
        {
            string filePath = @"D:\导入Excel\02-分部分项工程量清单-一标段-修订二(1).xls";
            DataTable dataTable = ExportHelper.ImportToExcel(filePath);
            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (DataColumn c in dataTable.Columns)
                {
                    string columnName = c.ColumnName;
                    object value = dr[c];
                }
            }

            return dataTable;
        }

        /// <summary>
        /// 上传文件流导入
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportToExcelV2")]
        public object ImportToExcelV2(IFormFile file)
        {
            DataTable dataTable = ExportHelper.ImportToExcel(file.OpenReadStream());
            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (DataColumn c in dataTable.Columns)
                {
                    string columnName = c.ColumnName;
                    object value = dr[c];
                }
            }
            return dataTable;
        }
        #endregion
        #endregion

        /// <summary>
        /// StringConvertAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("StringConvertAll")]
        public ActionResult StringConvertAll()
        {
            //List转字符串
            List<string> List = new List<string>();
            string strArray = string.Join(",", List);

            //字符串转List
            string str = "2,4,4,4";
            List = new List<string>(str.Split(','));

            //字符数组转Int数组
            int[] list = Array.ConvertAll<string, int>(str.Split(','), s => int.Parse(s));
            long[] cartIds = Array.ConvertAll<string, long>(str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), q => q.ToLong());
            string[] arr = str.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

            //List<string>字符串转Int数组
            List = new List<string>();
            strArray = string.Join(",", List);
            list = Array.ConvertAll<string, int>(strArray.Split(','), s => int.Parse(s));

            return Ok();
        }
    }
}
