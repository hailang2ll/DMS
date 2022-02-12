using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Authorizations.Model
{
    public class JwtSettingModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string SecretFile { get; set; }
    }
}
