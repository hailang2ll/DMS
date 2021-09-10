using DMS.Redis;
using System;

namespace DMS.Auth.Tickets
{
    public class RedisToken
    {
        public static RedisManager memCached = null;
        public string sid = null;
        public RedisToken(string sid)
        {
            this.sid = sid.Trim();
            if (memCached == null)
            {
                memCached = new RedisManager(0);
            }
        }

        private UserTicket userTicket;
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        public UserTicket UserTicket
        {
            get
            {
                //try
                //{
                userTicket = memCached.StringGet<UserTicket>(sid);
                if (userTicket == null)
                {
                    //获取用户票据为空，重新登录
                    var option = AppConfig.RedisOption;
                    userTicket.Msg = $"未找到SID，sid={sid},{memCached._conn.IsConnected},{option.RedisConnectionString}";
                    userTicket.Code = 3;
                    return userTicket;
                }

                DateTime dateExp = userTicket.ExpDate;
                DateTime dateNow = DateTime.Now;
                TimeSpan diff = dateNow - dateExp;
                long days = diff.Days;
                if (days > 90)
                {
                    //用户票据缓存90天,用户票据缓存时间超时，重新登录
                    memCached.KeyDelete(sid);
                    userTicket.Msg = string.Format("用户票据缓存时间超时，sid={0},userid={1},username={2},timeout=90天,days={3}", sid, userTicket.ID, userTicket.Name, days);
                    userTicket.Code = 5;
                    return userTicket;
                }
                else
                {
                    //获取用户票据成功，正常票据
                    userTicket.Msg = string.Format("{0}用户票据正常", userTicket.ID);
                    userTicket.Code = 4;
                    return userTicket;
                }
                //}
                //catch (Exception ex)
                //{
                //    userTicket.Msg = string.Format("用户票据正常异常{0}", ex.Message);
                //    userTicket.Code = 6;
                //    return userTicket;
                //}

            }
        }



    }
}
