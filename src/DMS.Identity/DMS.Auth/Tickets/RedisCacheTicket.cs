using System;
using System.Collections.Generic;
using System.Text;
using DMS.Redis;

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
                memCached = new RedisManager(0);
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
                if (string.IsNullOrWhiteSpace(sid))
                {
                    result.Msg = "获取sid为空";
                    return result;
                }

                if (memCached == null)
                {
                    //初始缓存对象为空，重新登录
                    result.Msg = string.Format("初始缓存对象为空，sid={0}", sid);
                    return result;
                }

                TicketEntity userTicket = memCached.StringGet<TicketEntity>(sid);
                if (userTicket == null)
                {
                    //获取用户票据为空，重新登录
                    result.Msg = string.Format("获取用户票据为空，sid={0}", sid);
                    return result;
                }

                if (!string.IsNullOrEmpty(userTicket.MemberName))
                {
                    //获取用户票据成功，但用户名称为空，重新登录
                    result.Msg = string.Format("获取用户票据成功，但用户名为空，sid={0}", sid);
                    return result;
                }

                DateTime dateExp = userTicket.ExpDate;
                DateTime dateNow = DateTime.Now;
                TimeSpan diff = dateNow - dateExp;
                long days = diff.Days;
                if (days > 30)
                {
                    //APP用户票据缓存30天,用户票据缓存时间超时，重新登录
                    memCached.KeyDelete(sid);
                    errMsg = string.Format("用户票据缓存时间超时，sid={0},userid={1},username={2},days={3}", sid, userTicket.MemberID, userTicket.MemberName, days);
                    result.Msg = errMsg;
                    return result;
                }
                else
                {
                    //获取用户票据成功，正常票据
                    //errMsg = string.Format("获取用户票据成功，正常票据,sid={0},memberid={1},membername={2},days={3}", sid, userTicket.MemberID, userTicket.MemberName, days);
                    //Log.Logger.Info(errMsg);
                    return userTicket;
                }
            }
        }



    }
}
