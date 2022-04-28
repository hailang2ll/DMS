using DMS.Api2.Authorizations.Model;

namespace DMS.Api2.Authorizations
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfig
    {

        /// <summary>
        /// 
        /// </summary>
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
