﻿using DMS.Authorizations.Model;
using DMS.Common.Extensions;
using DMS.Common.Helper;
using DMS.Common.JsonHandler;
using DMS.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DMS.Authorizations.Policys
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;
        private readonly DMS.Authorizations.UserContext.Jwt.IUserAuth _userAuth;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleModulePermissionServices"></param>
        /// <param name="accessor"></param>
        public PermissionRequirementHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor, DMS.Authorizations.UserContext.Jwt.IUserAuth userAuth)
        {
            Schemes = schemes;
            _accessor = accessor;
            _userAuth = userAuth;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _accessor.HttpContext;
            if (httpContext != null)
            {
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //获取授权方式
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    if (result.Succeeded)
                    {
                        if (result?.Principal != null)
                        {
                            long uid = _userAuth.Uid;
                            //long cid = _userAuth.Cid;
                            //string epcode = _userAuth.EpCode;
                            string token = _userAuth.GetToken();
                            //bool isAuth = _userAuth.IsAuthenticated();

                            httpContext.User = result.Principal;

                            #region 过期时间验证
                            var isExp = false;
                            if (Permissions.IsUseIds4)
                            {
                                isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) != null && DMS.Common.Extensions.GlobalExtensions.ToDate(httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) >= DateTime.Now;
                            }
                            else
                            {
                                //时间戳判断
                                isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Exp)?.Value) != null && DMS.Common.Extensions.DateTimeExtensions.StampToDateTime(httpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Exp)?.Value) >= DateTime.Now;
                                //时间判断用
                                //isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Exp)?.Value) != null && DMS.Common.Extensions.GlobalExtensions.ToDate(httpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Exp)?.Value) >= DateTime.Now;
                            }
                            if (!isExp)
                            {
                                ConsoleHelper.WriteInfoLine($"token已过期:{uid},{token}");
                                context.Fail();
                                return;
                            }
                            #endregion

                            #region 验证接口权限
                            var isMatchUrl = false;
                            var list = await _userAuth.GetPermissionDatas();
                            if (list != null && list.Count > 0)
                            {
                                requirement.Permissions = list;
                                //string permission = $"资源权限校验：uid={uid}，questUrl={questUrl}=====menuList={JsonSerializerExtensions.SerializeObject(list)}";
                                //ConsoleHelper.WriteInfoLine($"userauth:permission={permission}");

                                //判断角色与 Url 是否对应
                                var menu = requirement.Permissions.Where(x => x.Url.ToLower().Equals(questUrl)).FirstOrDefault();
                                if (menu != null)
                                {
                                    isMatchUrl = true;
                                }
                            }
                            if (!isMatchUrl)
                            {
                                ConsoleHelper.WriteInfoLine($"接口权限不足，未匹配到URL:{uid},{questUrl}");
                                context.Fail();
                                return;
                            }
                            else
                            {
                                //验证通过，追加时间
                                await _userAuth.SetTokenExpireAsync(token, DateTime.Now.Add(requirement.Expiration));
                                context.Succeed(requirement);
                                return;
                            }
                            #endregion

                        }
                    }

                }

                context.Fail();
            }
        }


        /// <summary>
        /// 第一版
        /// PermissionData(){Name="角色名"}
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        //protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        //{
        //    var httpContext = _accessor.HttpContext;

        //    #region 获取系统中所有的角色和菜单的关系集合
        //    //if (!requirement.Permissions.Any())
        //    //{
        //    //    //var data = await _roleModulePermissionServices.RoleModuleMaps();
        //    //    //List<PermissionData> data = new List<PermissionData>() {
        //    //    //    new PermissionData (){ Id = 1, Name="admin", LinkUrl = "http://localhost:20300",IsDeleted = false},
        //    //    //    new PermissionData (){ Id = 2, Name="admin", LinkUrl = "http://localhost:20300",IsDeleted = false},
        //    //    //    new PermissionData (){ Id = 3, Name="admin", LinkUrl = "http://localhost:20300",IsDeleted = false},
        //    //    //};

        //    //    var data = new List<PermissionData>() {
        //    //                  new PermissionData { Id=1,  LinkUrl="/api/Oauth2/GetProduct1", Name="invoice"},
        //    //                  new PermissionData { Id=2,  LinkUrl="/api/values", Name="admin"},
        //    //                  new PermissionData { Id=3,  LinkUrl="/api/Oauth2/GetProduct2", Name="system"},
        //    //                  new PermissionData { Id=4,  LinkUrl="/api/values1", Name="system"}
        //    //};

        //    //    var list = new List<PermissionItem>();
        //    //    if (Permissions.IsUseIds4)
        //    //    {
        //    //        list = (from item in data
        //    //                where item.IsDeleted == false
        //    //                orderby item.Id
        //    //                select new PermissionItem
        //    //                {
        //    //                    Url = item.LinkUrl,
        //    //                    Name = item.Name.ToStringDefault(),
        //    //                }).ToList();
        //    //    }
        //    //    else
        //    //    {
        //    //        //jwt
        //    //        list = (from item in data
        //    //                where item.IsDeleted == false
        //    //                orderby item.Id
        //    //                select new PermissionItem
        //    //                {
        //    //                    Url = item.LinkUrl,
        //    //                    Name = item.Name,
        //    //                }).ToList();
        //    //    }
        //    //    requirement.Permissions = list;
        //    //}
        //    #endregion
        //    if (httpContext != null)
        //    {
        //        var questUrl = httpContext.Request.Path.Value.ToLower();

        //        // 整体结构类似认证中间件UseAuthentication的逻辑，具体查看开源地址
        //        // https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/Core/src/AuthenticationMiddleware.cs
        //        httpContext.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
        //        {
        //            OriginalPath = httpContext.Request.Path,
        //            OriginalPathBase = httpContext.Request.PathBase
        //        });

        //        // Give any IAuthenticationRequestHandler schemes a chance to handle the request
        //        // 主要作用是: 判断当前是否需要进行远程验证，如果是就进行远程验证
        //        var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        //        foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
        //        {
        //            if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
        //            {
        //                context.Fail();
        //                return;
        //            }
        //        }


        //        //判断请求是否拥有凭据，即有没有登录
        //        var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
        //        if (defaultAuthenticate != null)
        //        {
        //            var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
        //            //result?.Principal不为空即登录成功
        //            if (result?.Principal != null)
        //            {
        //                #region 验证权限
        //                httpContext.User = result.Principal;
        //                // 获取当前用户的角色信息
        //                var currentUserRoles = new List<string>();
        //                if (Permissions.IsUseIds4)
        //                {
        //                    currentUserRoles = (from item in httpContext.User.Claims
        //                                        where item.Type == "role"
        //                                        select item.Value).ToList();
        //                }
        //                else
        //                {
        //                    //jwt
        //                    currentUserRoles = (from item in httpContext.User.Claims
        //                                        where item.Type == requirement.ClaimType
        //                                        select item.Value).ToList();
        //                }

        //                var isMatchRole = false;
        //                var permisssionRoles = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Name));
        //                string permission = $"questUrl={questUrl}=====permisssionRoles={JsonSerializerExtensions.SerializeObject(permisssionRoles.ToList())}";
        //                ConsoleHelper.WriteInfoLine($"userauth:permission={permission}");
        //                foreach (var item in permisssionRoles)
        //                {
        //                    try
        //                    {
        //                        if (Regex.Match(questUrl, item.Url?.ToStringDefault().ToLower())?.Value == questUrl)
        //                        {
        //                            isMatchRole = true;
        //                            break;
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        // ignored
        //                    }
        //                }

        //                //当前用户无角色||未匹配到角色与URL，认证失败
        //                if (currentUserRoles.Count <= 0 || !isMatchRole)
        //                {
        //                    ConsoleHelper.WriteInfoLine($"userauth:当前用户无角色||未匹配到角色与URL");
        //                    context.Fail();
        //                    return;
        //                }
        //                #endregion

        //                #region 过期时间验证
        //                var isExp = false;
        //                if (Permissions.IsUseIds4)
        //                {
        //                    isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) != null && DMS.Common.Extensions.DateTimeExtensions.ToDateTime(httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) >= DateTime.Now;
        //                }
        //                else
        //                {
        //                    //jwt
        //                    //isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now;
        //                    isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == "expiration")?.Value) != null && DMS.Common.Extensions.DateTimeExtensions.ToDateTime(httpContext.User.Claims.SingleOrDefault(s => s.Type == "expiration")?.Value) >= DateTime.Now;

        //                }
        //                if (isExp)
        //                {
        //                    context.Succeed(requirement);
        //                }
        //                else
        //                {
        //                    ConsoleHelper.WriteInfoLine($"userauth:时间已过期");
        //                    context.Fail();
        //                    return;
        //                }
        //                #endregion
        //                return;
        //            }
        //        }

        //        //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
        //        //if (!(questUrl.Equals(requirement.DeniedAction.ToLower(), StringComparison.Ordinal)
        //        //    && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)))
        //        //{
        //        //    ConsoleHelper.WriteInfoLine($"userauth:访问的登录url");
        //        //    context.Fail();
        //        //    return;
        //        //}
        //    }
        //}
    }
}
