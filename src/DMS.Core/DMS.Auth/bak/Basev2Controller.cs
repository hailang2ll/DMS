using DMS.Common.Model.Result;
using DMS.Redis;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Auth
{
    public class Basev2Controller : ControllerBase
    {
        private readonly IRedisRepository _redisRepository;
        public Basev2Controller(IRedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }
        //public RedisManager redisManager { get; set; }
        //public RedisManager RedisManager
        //{
        //    get
        //    {
        //        if (redisManager == null)
        //        {
        //            redisManager = new RedisManager(0);
        //        }
        //        return redisManager;
        //    }
        //}
        public UserTicket UserTicket
        {
            get
            {
                var token = HttpContext.Request.Headers["AccessToken"].ToString();

                var userTicket = _redisRepository.GetValueAsync<UserTicket>(token).Result;// RedisManager.StringGet<UserTicket>(token);
                if (userTicket != null && userTicket.ID > 0)
                {
                    return userTicket;
                }
                return null;
            }
        }
        //[NonAction]
        //public (bool, ResponseResult) ChenkLogin()
        //{
        //    if (UserTicket.ID <= 0)
        //    {
        //        var response = new ResponseResult()
        //        {
        //            errno = 30,
        //            errmsg = "请先登录"
        //        };
        //        return (false, response);
        //    }
        //    return (true, new ResponseResult() { errmsg = "success" });
        //}
        [NonAction]
        public (bool, ResponseResult<T>) ChenkLogin<T>()
        {
            if (UserTicket.ID <= 0)
            {
                var response = new ResponseResult<T>()
                {
                    errno = 30,
                    errmsg = "请先登录"
                };
                return (false, response);
            }
            return (true, new ResponseResult<T>() { errmsg = "success" });
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

    }
}
