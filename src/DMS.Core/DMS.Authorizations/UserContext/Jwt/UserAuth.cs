﻿using DMS.Authorizations.Model;
using DMS.Authorizations.Policys;
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

        public long Uid => GetClaimValueByType(JwtClaimTypes.Uid).FirstOrDefault().ToLong();
        public long Cid => GetClaimValueByType(JwtClaimTypes.Cid).FirstOrDefault().ToLong();

        public string EpCode
        {
            get
            {
                return GetClaimValueByType(JwtClaimTypes.EpCode).FirstOrDefault();

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
            List<PermissionItem> list = await _redisRepository.HashGeAsync<List<PermissionItem>>(token, "permission");
            if (list == null)
            {
                return new List<PermissionItem>();
            }
            return list;
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