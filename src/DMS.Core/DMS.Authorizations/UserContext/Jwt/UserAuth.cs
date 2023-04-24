using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
using DMS.Authorizations.UserContext.Dto;
using DMS.Common.Extensions;
using DMS.Common.Helper;
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
            this._accessor = httpContextAccessor;
            this._redisRepository = redisRepository;
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
        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["Authorization"].ToStringDefault().Replace("Bearer ", "");
        }
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }


        /// <summary>
        /// 获取缓存资源列表
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
            return null;
        }
        /// <summary>
        /// 设置token过期时间
        /// </summary>
        /// <param name="token"></param>
        /// <param name="exprise"></param>
        /// <returns></returns>
        public async Task<bool> SetTokenExpireAsync(string token, DateTime exprise)
        {
            bool isFlag = await _redisRepository.ExistAsync(token);
            if (isFlag)
            {
                await _redisRepository.HashSetAsync(token, "exptime", exprise);
                return await _redisRepository.KeyExpireAsync(token, exprise);
            }
            return false;
        }

        #region private

        private UserClaimModel UserClaimModel
        {
            get
            {
                //可以先验证缓存是否存在token
                string UniqueId = GetClaimValueByType(JwtClaimTypes.UuId).FirstOrDefault();
                if (!UniqueId.IsNullOrEmpty())
                {
                    UniqueId = DMS.Common.Encrypt.DESHelper.Decode(UniqueId);
                    ConsoleHelper.WriteInfoLine($"获取jwt.token内容-UserClaimModel:{UniqueId}");
                    string[] arr = UniqueId.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                    UserClaimModel claimModel = new UserClaimModel()
                    {
                        Uid = arr.Length > 0 ? arr[0].ToLong() : 0,
                        Cid = arr.Length > 1 ? arr[1].ToLong() : 0,
                        EpCode = arr.Length > 2 ? arr[2].ToStringDefault() : "",
                    };
                    return claimModel;
                }
                return new UserClaimModel();
            }
        }

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
