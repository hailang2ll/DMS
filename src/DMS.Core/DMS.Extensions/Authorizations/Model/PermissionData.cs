using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Extensions.Authorizations.Model
{
    public class PermissionData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public bool IsDeleted { get; set; }

    }
}
