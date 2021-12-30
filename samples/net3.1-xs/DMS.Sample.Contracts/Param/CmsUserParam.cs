using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DMS.Sample.Contracts.Param
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginCmsUserParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string UserPassword { get; set; }
    }
}
