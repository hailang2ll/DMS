using DMS.Redis;
using DMSN.Common.BaseResult;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DMS.Auth
{
    public class UserAuth : IUserAuth
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisRepository redisRepository;
        public UserAuth(IHttpContextAccessor httpContextAccessor, IRedisRepository redisRepository)
        {
            Console.WriteLine($"UserAuth:{redisRepository}");
            this._accessor = httpContextAccessor;
            this.redisRepository = redisRepository;
            Console.WriteLine($"UserAuth.1:{redisRepository}");
        }

        public string Name => UserTicket.Name;

        public long ID => UserTicket.ID;

        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["AccessToken"].ToString();
        }

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
                var userTicket = redisRepository.GetValueAsync<UserTicket>(token).Result;// RedisManager.StringGet<UserTicket>(token);
                if (userTicket != null && userTicket.ID > 0)
                {
                    return userTicket;
                }
                return new UserTicket() { Msg = "get userticket fail" };
            }
        }



    }
}
