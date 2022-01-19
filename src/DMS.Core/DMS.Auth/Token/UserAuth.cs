using DMS.Redis;
using DMS.Common.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Auth.Token
{
    public class UserAuth : IUserAuth
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisRepository redisRepository;
        public UserAuth(IHttpContextAccessor httpContextAccessor, IRedisRepository redisRepository)
        {
            Console.WriteLine($"DMS.Auth.Token-1:{redisRepository}");
            this._accessor = httpContextAccessor;
            this.redisRepository = redisRepository;
            Console.WriteLine($"DMS.Auth.Token-2:{redisRepository}");
        }
        public long ID => UserTicket.ID;
        public string EpCode => UserTicket.EpCode;
        public string UID => UserTicket.UID;
        public string Name => UserTicket.Name;
        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["AccessToken"].ToStringDefault().Replace("Bearer ", "");
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
                return new UserTicket();
            }
        }
    }
}
