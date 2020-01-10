using DMS.Redis;
using System;

namespace DMS.Auth.Tickets
{
    public class RedisCacheTicket
    {
        public static RedisManager memCached = null;
        public string sid = null;
        public RedisCacheTicket(string sid)
        {
            this.sid = sid;
            if (memCached == null)
            {
                memCached = new RedisManager(0);
            }
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        public TicketEntity CurrentUserTicket
        {
            get
            {
                string errMsg = "";
                TicketEntity result = new TicketEntity();
                try
                {
                    if (string.IsNullOrWhiteSpace(sid))
                    {
                        result.Msg = "获取sid为空";
                        result.Code = 1;
                        return result;
                    }

                    if (memCached == null)
                    {
                        //初始缓存对象为空，重新登录
                        result.Msg = string.Format("初始缓存对象为空，sid={0}", sid);
                        result.Code = 2;
                        return result;
                    }
                    var userTicket = memCached.StringGet<TicketEntity>(sid);
                    if (userTicket == null)
                    {
                        //获取用户票据为空，重新登录
                        var option = AppConfig.RedisOption;
                        result.Msg = $"未找到SID，sid={sid},{option.RedisConnectionString}";
                        result.Code = 3;
                        return result;
                    }

                    DateTime dateExp = userTicket.ExpDate;
                    DateTime dateNow = DateTime.Now;
                    TimeSpan diff = dateNow - dateExp;
                    long days = diff.Days;
                    if (days > 90)
                    {
                        //用户票据缓存90天,用户票据缓存时间超时，重新登录
                        memCached.KeyDelete(sid);
                        errMsg = string.Format("用户票据缓存时间超时，sid={0},userid={1},username={2},timeout=90天,days={3}", sid, userTicket.ID, userTicket.Name, days);
                        result.Msg = errMsg;
                        result.Code = 5;
                        return result;
                    }
                    else
                    {
                        //获取用户票据成功，正常票据
                        userTicket.Msg = string.Format("{0}用户票据正常", userTicket.ID);
                        userTicket.Code = 4;
                        result = userTicket;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    result.Msg = string.Format("用户票据正常异常{0}", ex.Message);
                    result.Code = 6;
                    return result;
                }

            }
        }



    }
}
