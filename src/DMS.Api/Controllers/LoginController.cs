using DMS.Api.Model;
using DMS.Authorizations;
using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
using DMS.Authorizations.UserContext;
using DMS.Authorizations.UserContext.Dto;
using DMS.Common.JsonHandler;
using DMS.Common.Model.Result;
using DMS.Models;
using DMS.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// 我是登录
    /// 颁发令牌
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly PermissionRequirement _requirement;
        /// <summary>
        /// 
        /// </summary>
        private readonly IRedisRepository _redisRepository;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="requirement"></param>
        /// <param name="redisRepository"></param>
        public LoginController(PermissionRequirement requirement, IRedisRepository redisRepository)
        {
            _requirement = requirement;
            _redisRepository = redisRepository;
        }
        /// <summary>
        /// 普通TOKEN认识方式
        /// </summary>
        /// <returns></returns>
        [HttpPost("TokenLogin")]
        public async Task<ResponseResult> TokenLogin()
        {
            ResponseResult result = new ResponseResult();
            UserTicket tokenModel = new UserTicket
            {
                ID = 120,
                EpCode = "100214545454",
                UID = "435353534",
                ExpDate = DateTime.Now.AddDays(1),
            };
            string sid = DMS.Extensions.UniqueGenerator.UniqueHelper.GetWorkerID().ToString();
            await _redisRepository.SetAsync(sid, tokenModel.SerializeObject(), tokenModel.ExpDate);
            result.data = sid;
            return result;
        }


        /// <summary>
        /// 登录 token生成
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ResponseResult> Login()
        {
            ResponseResult result = new ResponseResult();

            var userList = new List<YxyMember> {
                new YxyMember{  Id=11111,MemberName="aaa",Password="123456"},
                new YxyMember{  Id=22222,MemberName="bbb",Password="123456"},
            };

            var user = userList.SingleOrDefault(q => q.MemberName == "bbb" && q.Password == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                #region 1
                ////var userRoles = "admin,invoice";
                ////如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                ////需要改进：JwtClaimTypes，减少生成规则
                //var claims = new List<Claim> {
                //    new Claim(JwtClaimTypes.Uid, user.Id.ToString()),
                //    new Claim(JwtClaimTypes.Cid, "555555555"),
                //    new Claim(JwtClaimTypes.EpCode, "6666666666666"),
                //    new Claim(JwtClaimTypes.Expiration, DateTime.Now.Add(_requirement.Expiration).ToString())
                //};
                ////claims.AddRange(userRoles.Split(',').Select(s => new Claim(JwtClaimTypes.Role, s)));
                //var jwtToken = JwtHelper.Create(claims.ToArray());
                #endregion

                #region 2
                UserClaimModel claimModel = new UserClaimModel()
                {
                    Uid = user.Id.ToString(),
                    Cid = user.Id.ToString(),
                    EpCode = user.Id.ToString(),
                    Expiration = DateTime.Now.Add(_requirement.Expiration).ToString(),
                };
                var jwtToken = JwtHelper.Create(claimModel);
                #endregion

                var token = jwtToken.token;
                result.data = new
                {
                    companyList = new List<dynamic>
                    {
                        new { cid=1,cname="A公司" },
                        new { cid=2,cname="B公司" },
                    },
                    token = token,
                    user = new
                    {
                        id = 1,
                        name = "hailang"
                    },
                };





                #region 用户信息缓存
                var userRedis = new
                {
                    Uid = user.Id,
                    Cid = 5555,//当前公司
                    Expiration = jwtToken.expires,
                };
                await _redisRepository.HashSetAsync(token, "user", userRedis);
                #endregion

                #region 用户菜单缓存,多角色合并菜单
                List<MenuCompanyModel> menuRedis = new List<MenuCompanyModel>();
                MenuCompanyModel menuCompany1 = new MenuCompanyModel()
                {
                    Cid = 1,
                    Name = "A公司",
                    Roles = "admin,system",
                    ChildNodes = new List<MenuModel>() {
                        new MenuModel()
                        {
                            Id=1,
                            Type=10,
                            Name="系统中心",
                            Url="",
                            IconName="system",
                            ChildNodes=new List<MenuModel>()
                            {
                                new MenuModel()
                                {
                                    Id=1,
                                    Type=20,
                                    Name="权限管理",
                                    Url="",
                                    IconName="",
                                    ChildNodes=new List<MenuModel>()
                                    {
                                        new MenuModel()
                                        {  
                                            Id=1,
                                            Type=30,
                                            Name="用户列表",
                                            Url="/api/add",
                                            IconName="",
                                            ChildNodes=null
                                        },
                                        new MenuModel()
                                        {
                                            Id=2,
                                            Type=30,
                                            Name="角色列表",
                                            Url="/api/add",
                                            IconName="",
                                            ChildNodes=null
                                        },
                                        new MenuModel()
                                        {
                                            Id=3,
                                            Type=30,
                                            Name="部门列表",
                                            Url="/api/add",
                                            IconName="",
                                            ChildNodes=null
                                        }
                                    }
                                },
                            },
                        },
                    },
                };
                MenuCompanyModel menuCompany2 = new MenuCompanyModel()
                {
                    Cid = 2,
                    Name = "B公司",
                    Roles = "admin,system",
                    ChildNodes = new List<MenuModel>() {
                        new MenuModel()
                        {
                            Id=1,
                            Type=10,
                            Name="系统中心",
                            Url="",
                            IconName="system",
                            ChildNodes=new List<MenuModel>()
                            {
                                new MenuModel()
                                {
                                    Id=1,
                                    Type=20,
                                    Name="权限管理",
                                    Url="",
                                    IconName="",
                                    ChildNodes=new List<MenuModel>()
                                    {
                                        new MenuModel()
                                        {
                                            Id=1,
                                            Type=30,
                                            Name="用户列表",
                                            Url="",
                                            IconName="",
                                            ChildNodes=null
                                        },
                                        new MenuModel()
                                        {
                                            Id=2,
                                            Type=30,
                                            Name="角色列表",
                                            Url="",
                                            IconName="",
                                            ChildNodes=null
                                        },
                                        new MenuModel()
                                        {
                                            Id=3,
                                            Type=30,
                                            Name="部门列表",
                                            Url="",
                                            IconName="",
                                            ChildNodes=null
                                        }
                                    }
                                },
                            },
                        },
                    },
                };
                menuRedis.Add(menuCompany1);
                menuRedis.Add(menuCompany2);
                await _redisRepository.HashSetAsync(token, "menu", menuRedis);
                #endregion

                #region 接口权限缓存
                var permissionRedis = new List<PermissionItem>
                {
                    new PermissionItem { Id=1,  Url="/api/Oauth2/GetProduct1"},
                    new PermissionItem { Id=2,  Url="/api/Oauth2/GetProduct"},
                    new PermissionItem { Id=3,  Url="/api/values"},
                };
                await _redisRepository.HashSetAsync(token, "permission", permissionRedis);
                await _redisRepository.KeyExpireAsync(token, jwtToken.expires);
                #endregion
            }
            return result;
        }


        /// <summary>
        /// JWT token 生成
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetJWTToken")]
        public ResponseResult GetJWTToken()
        {
            ResponseResult result = new ResponseResult();

            var userList = new List<dynamic> {
                new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                new { Id="45", UserName="bbb",Pwd="456789",Role="invoice"},
            };

            var user = userList.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                var userRoles = userList.Select(q => q.Role).ToList();
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                //需要改进：JwtClaimTypes，减少生成规则
                var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    //new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sid, DMS.Common.Encrypt.DESHelper.Encode("aaaa")),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString())
                };
                claims.AddRange(userRoles.Select(s => new Claim(ClaimTypes.Role, s)));

                if (!Permissions.IsUseIds4)
                {
                    //jwt
                    //var data = await _roleModulePermissionServices.RoleModuleMaps();
                    var data = new List<PermissionData>() {
                              new PermissionData { Id=1,  LinkUrl="/api/Oauth2/GetProduct1", Name="invoice"},
                              new PermissionData { Id=2,  LinkUrl="/api/values", Name="admin"},
                              new PermissionData { Id=3,  LinkUrl="/api/Oauth2/GetProduct2", Name="system"},
                              new PermissionData { Id=4,  LinkUrl="/api/values1", Name="system"}
                              };
                    var list = (from item in data
                                where item.IsDeleted == false
                                orderby item.Id
                                select new PermissionItem
                                {
                                    Url = item.LinkUrl,
                                    //Name = item.Name.ToStringDefault(),
                                }).ToList();


                    //DMS.Extensions.Authorizations.AppConfig.Audience = user.UserName + DateTime.Now.ToString();
                    //_requirement.Audience = DMS.Extensions.Authorizations.AppConfig.Audience;
                    _requirement.Permissions = list;
                }

                var token = JwtHelper.Create(claims.ToArray(), _requirement);
                result.data = token;

            }
            return result;
        }

        /// <summary>
        /// 请求刷新Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("RefreshToken")]
        public ResponseResult RefreshToken(string token = "")
        {
            ResponseResult result = new ResponseResult();

            if (string.IsNullOrEmpty(token))
            {
                result.errno = 1;
                result.errmsg = "token无效，请重新登录";
                return result;
            }
            var tokenModel = JwtHelper.SerializeJwt(token);
            if (tokenModel == null || JwtHelper.customSafeVerify(token) && tokenModel.Uid < 0)
            {
                result.errno = 4;
                result.errmsg = "认证失败";
                return result;
            }
            else
            {
                var userList = new List<dynamic> {
                    new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                    new { Id="45", UserName="bbb",Pwd="456789",Role="invoice"},
                };
                var user = userList.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
                if (user == null)
                {
                    result.errno = 1;
                    result.errmsg = "用户名或密码错误";
                    return result;
                }
                else
                {
                    var userRoles = userList.Select(q => q.Role).ToList();
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                    //claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));
                    claims.AddRange(userRoles.Select(s => new Claim(ClaimTypes.Role, s)));
                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtHelper.Create(claims.ToArray(), _requirement);
                    result.data = refreshToken;
                    return result;
                }
            }
        }
        /// <summary>
        /// 主方法认证
        /// </summary>
        /// <returns></returns>
        [HttpPost("MainLogin")]
        public ResponseResult MainLogin()
        {
            ResponseResult result = new();

            var list = new List<dynamic> {
                new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                new { Id="45", UserName="bbb",Pwd="456789",Role="system"},
            };

            var user = list.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                var userRoles = list.Select(q => q.Role).ToList();
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(TimeSpan.FromSeconds(60 * 60).TotalSeconds).ToString()) };

                claims.AddRange(userRoles.Select(s => new Claim(ClaimTypes.Role, s)));
                var token = JwtHelper.Create(claims.ToArray(), _requirement);

                result.data = token;
            }
            return result;
        }

        /// <summary>
        /// LoginByCookies 认证
        /// </summary>
        /// <returns></returns>
        [HttpPost("LoginByCookies")]
        public ResponseResult LoginByCookies()
        {
            ResponseResult result = new();

            var list = new List<dynamic> {
                new { Id="12", UserName="aaa",Pwd="123456",Role="admin"},
                new { Id="45", UserName="bbb",Pwd="456789",Role="system"},
            };

            var user = list.SingleOrDefault(q => q.UserName == "aaa" && q.Pwd == "123456");
            if (user == null)
            {
                result.errno = 1;
                result.errmsg = "用户名或密码错误";
                return result;
            }
            else
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);//一定要声明AuthenticationScheme
                identity.AddClaim(new Claim(ClaimTypes.PrimarySid, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                HttpContext.SignInAsync(identity.AuthenticationType, new ClaimsPrincipal(identity));

                return result;
            }

        }
    }
}
