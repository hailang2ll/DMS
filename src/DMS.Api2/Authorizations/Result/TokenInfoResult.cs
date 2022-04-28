namespace DMS.Api2.Authorizations.Result
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenInfoResult
    {
        public bool success { get; set; }
        public string token { get; set; }
        public DateTime expires { get; set; }
        public string token_type { get; set; }
    }
}
