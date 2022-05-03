using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
using DMS.Authorizations.UserContext.Dto;
using DMS.Common.Extensions;
using DMS.Redis;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Authorizations.UserContext.Jwt
{
    public class UserAuth : IUserAuth
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisRepository _redisRepository;
        public UserAuth(IHttpContextAccessor httpContextAccessor, IRedisRepository redisRepository)
        {
            Console.WriteLine($"DMS.Auth.Oauth:{redisRepository}");
            this._accessor = httpContextAccessor;
            this._redisRepository = redisRepository;
        }

        public UserClaimModel UserClaimModel
        {
            get
            {
                string UniqueId = GetClaimValueByType(JwtClaimTypes.UniqueId).FirstOrDefault();
                if (!UniqueId.IsNullOrEmpty())
                {
                    string[] arr = UniqueId.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    UserClaimModel claimModel = new UserClaimModel()
                    {
                        Uid = arr[0].ToLong(),
                        Cid = arr[1].ToLong(),
                        EpCode = arr[2],
                        Role = arr[3],
                        Expiration = arr[4],
                    };
                    return claimModel;
                }
                return new UserClaimModel();
            }
        }
        public long Uid => UserClaimModel.Uid;
        public long Cid => UserClaimModel.Cid;

        public string EpCode
        {
            get
            {
                return UserClaimModel.EpCode;

                //string sid = DMS.Common.Encrypt.DESHelper.Decode(Sid);
                //var list = new List<string>(sid.Split('^'));
                //return list[index];
            }
        }
        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionItem>> GetPermissionDatas()
        {
            var token = GetToken();
            bool flag = await _redisRepository.HashExistsAsync(token, "permission");
            if (flag)
            {
                List<PermissionItem> list = await _redisRepository.HashGeAsync<List<PermissionItem>>(token, "permission");
                if (list != null && list.Count > 0)
                {
                    return list;
                }
            }
            return new List<PermissionItem>();
        }
        /// <summary>
        /// 设置token过期时间
        /// </summary>
        /// <param name="token"></param>
        /// <param name="exprise"></param>
        /// <returns></returns>
        public async Task<bool> SetTokenExpireAsync(string token, DateTime exprise)
        {
            await _redisRepository.HashSetAsync(token, "exptime", exprise);
            return await _redisRepository.KeyExpireAsync(token, exprise);
        }



        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["Authorization"].ToStringDefault().Replace("Bearer ", "");
        }
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }


        #region private

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
        #endregion

    }
}
