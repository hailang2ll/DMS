using DMS.Auth.Tickets;
using DMS.Common.BaseResult;
using DMS.Redis;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Auth
{
    public class BaseController : ControllerBase
    {
        public RedisManager redisManager { get; set; }
        public RedisManager RedisManager
        {
            get
            {
                if (redisManager == null)
                {
                    redisManager = new RedisManager(0);
                }
                return redisManager;
            }
        }
        public UserTicket UserTicket
        {
            get
            {
                var token = HttpContext.Request.Headers["AccessToken"].ToString();
                var userTicket = RedisManager.StringGet<UserTicket>(token);
                if (userTicket != null && userTicket.ID > 0)
                {
                    return userTicket;
                }
                return new UserTicket() { Msg = "get userticket fail" };
            }
        }
        [NonAction]
        public (bool, ResponseResult) ChenkLogin(UserTicket userTicket)
        {
            if (userTicket.ID <= 0)
            {
                var response = new ResponseResult()
                {
                    errno = 30,
                    errmsg = "请先登录"
                };
                return (false, response);
            }
            return (true, new ResponseResult() { errmsg = "success" });
        }
        [NonAction]
        public (bool, ResponseResult<T>) ChenkLogin<T>(UserTicket userTicket)
        {
            if (userTicket.ID <= 0)
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

        //public Task<(bool, ResponseResult<T>)> ChenkLoginTask<T>(UserTicket userTicket)
        //{
        //    if (userTicket.ID <= 0)
        //    {
        //        var response = new ResponseResult<T>()
        //        {
        //            errno = 30,
        //            errmsg = "请先登录"
        //        };
        //        return Task.FromResult((false, response));
        //    }
        //    return Task.FromResult((true, new ResponseResult<T>() { errmsg = "success" }));
        //}

    }
}
