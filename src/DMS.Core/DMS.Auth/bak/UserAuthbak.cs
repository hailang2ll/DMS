using DMS.Auth.Oauth2;
using DMS.Common.Extensions;
using DMS.Common.Model.Result;
using DMS.Redis;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Auth
{
    public class UserAuthbak : IUserAuth
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisRepository redisRepository;
        public UserAuthbak(IHttpContextAccessor httpContextAccessor, IRedisRepository redisRepository)
        {
            Console.WriteLine($"UserAuth:{redisRepository}");
            this._accessor = httpContextAccessor;
            this.redisRepository = redisRepository;
            Console.WriteLine($"UserAuth.1:{redisRepository}");
        }

        public string Name => UserTicket.Name;

        public long ID => UserTicket.ID;
        public (bool, ResponseResult) ChenkLogin()
        {
            return ChenkLoginAsync().Result;
        }
        public (bool, ResponseResult<T>) ChenkLogin<T>()
        {
            return ChenkLoginAsync<T>().Result;
        }
        public Task<(bool, ResponseResult)> ChenkLoginAsync()
        {
            if (UserTicket.ID <= 0)
            {
                var response = new ResponseResult()
                {
                    errno = 30,
                    errmsg = "请先登录"
                };
                return Task.FromResult((false, response));
            }
            return Task.FromResult((true, new ResponseResult() { errmsg = "success" }));
        }
        public Task<(bool, ResponseResult<T>)> ChenkLoginAsync<T>()
        {
            if (UserTicket.ID <= 0)
            {
                var response = new ResponseResult<T>()
                {
                    errno = 30,
                    errmsg = "请先登录"
                };
                return Task.FromResult((false, response));
            }
            return Task.FromResult((true, new ResponseResult<T>() { errmsg = "success" }));
        }
        public UserTicket UserTicket
        {
            get
            {
                var token = _accessor.HttpContext.Request.Headers["AccessToken"].ToString();
                var userTicket = redisRepository.GetValueAsync<UserTicket>(token).Result;
                if (userTicket != null && userTicket.ID > 0)
                {
                    return userTicket;
                }
                return null;
            }
        }




        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["Authorization"].ToStringDefault().Replace("Bearer ", "");
        }
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string Name2 => GetName();

        private string GetName()
        {
            if (IsAuthenticated() && !_accessor.HttpContext.User.Identity.Name.IsNullOrEmpty())
            {
                //var v = _accessor.HttpContext.User.Claims.Where(q => q.Type == System.Security.Claims.ClaimTypes.PrimarySid).FirstOrDefault().Value;
                return _accessor.HttpContext.User.Identity.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(GetToken()))
                {
                    var getNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
                    return GetUserInfoFromToken(getNameType).FirstOrDefault().ToStringDefault();
                }
            }

            return "";
        }

        public long ID2 => GetClaimValueByType("jti").FirstOrDefault().ToLong();

        public List<string> GetUserInfoFromToken(string ClaimType)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = "";

            token = GetToken();
            // token校验
            if (!token.IsNullOrEmpty() && jwtHandler.CanReadToken(token))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);

                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }

            return new List<string>() { };
        }
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
        public List<string> GetClaimValueByType(string ClaimType)
        {

            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();

        }

    }
}
