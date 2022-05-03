using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.UserContext.Dto
{
    /// <summary>
    /// 身份信息
    /// </summary>
    public class UserClaimModel
    {
        public long Uid { get; set; }
        public long Cid { get; set; }
        public string EpCode { get; set; }
        public string Role { get; set; }
        public string Expiration { get; set; }
    }
}
