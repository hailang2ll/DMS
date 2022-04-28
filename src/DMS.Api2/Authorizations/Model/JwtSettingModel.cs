namespace DMS.Api2.Authorizations.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtSettingModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string SecretFile { get; set; }
        public double ExpireMinutes { get; set; }
    }
}
