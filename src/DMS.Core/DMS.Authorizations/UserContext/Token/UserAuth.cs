﻿using DMS.Redis;
using DMS.Common.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.UserContext.Token
{
    public class UserAuth : IUserAuth
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisRepository redisRepository;
        public UserAuth(IHttpContextAccessor httpContextAccessor, IRedisRepository redisRepository)
        {
            Console.WriteLine($"DMS.Auth.Token:{redisRepository}");
            this._accessor = httpContextAccessor;
            this.redisRepository = redisRepository;
        }
        public long ID => UserTicket.ID;
        public string EpCode => UserTicket.EpCode;
        public string UID => UserTicket.UID;
        public string Name => UserTicket.Name;
        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["AccessToken"].ToStringDefault().Replace("Bearer ", "");
        }

        private UserTicket _userTicket;
        public UserTicket UserTicket
        {

            get
            {
                if (_userTicket == null)
                {
                    var token = _accessor.HttpContext.Request.Headers["AccessToken"].ToString();
                    var userTicket = redisRepository.GetValueAsync<UserTicket>(token).Result;
                    if (userTicket != null && userTicket.ID > 0)
                    {
                        DateTime dateExp = userTicket.ExpDate;
                        DateTime dateNow = DateTime.Now;
                        TimeSpan diff = dateNow - dateExp;
                        long days = diff.Days;
                        if (days > 90)
                        {
                            //用户票据缓存90天,用户票据缓存时间超时，重新登录
                            //memCached.KeyDelete(sid);
                        }
                        else
                        {
                            //获取用户票据成功，正常票据
                        }
                        _userTicket = userTicket;
                    }
                    else
                    {
                        _userTicket = new UserTicket();
                    }
                    return _userTicket;
                }
                return _userTicket;
            }
            set => _userTicket = value;
        }
    }
}
