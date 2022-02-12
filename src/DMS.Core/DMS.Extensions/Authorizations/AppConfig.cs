using DMS.Extensions.Authorizations.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Authorizations
{
    public class AppConfig
    {
        //public static string Issuer { get; set; } = "DMS.Core";
        //public static string Audience { get; set; } = "wr";
        //public static string SecretKey = "sdfsdfsrty45634kkhllghtdgdfss345t678fs";

        public static JwtSettingModel JwtSettingOption
        {
            get
            {
                JwtSettingModel settingModel = DMS.Common.AppConfig.GetValue<JwtSettingModel>("JwtSetting");
                return settingModel;
            }

        }
    }
}
