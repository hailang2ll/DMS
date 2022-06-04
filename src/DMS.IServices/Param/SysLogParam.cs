using DMS.Common.Model.Param;
using System;
using System.ComponentModel.DataAnnotations;

namespace DMS.IServices.Param
{
    /// <summary>
    /// 添加系统日志
    /// </summary>
    public class AddSysLogParam
    {
        [Required]
        public string MemberName { get; set; }
        [Required]
        public int SubSysid { get; set; }
        [Required]
        public string SubSysname { get; set; }
    }

}
