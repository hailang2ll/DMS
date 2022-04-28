using DMS.IServices.Param;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DMS.IServices.Validation.Attribute
{
    /// <summary>
    /// 自定义属性验证
    /// </summary>
    public class AddMemberParamValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userDto = (AddMemberParam)validationContext.ObjectInstance;   //获取类的实例对象            //验证用户名不能为空
            if (string.IsNullOrWhiteSpace(userDto.MemberName))
            {
                return new ValidationResult("用户名不能为空", new[] { nameof(userDto.MemberName) });
            }
            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                return new ValidationResult("密码不能为空", new[] { nameof(userDto.Password) });
            }
            if (string.IsNullOrWhiteSpace(userDto.Mobile))
            {
                return new ValidationResult("手机号不能为空", new[] { nameof(userDto.Mobile) });
            }
            if (!string.IsNullOrWhiteSpace(userDto.Mobile))
            {
                var regex = new Regex(@"^1[3456789]\d{9}$"); if (!regex.IsMatch(userDto.Mobile)) return new ValidationResult("手机号不符合规则12", new[] { nameof(userDto.Mobile) });
            }
            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                // 必须包含数字
                //必须包含小写或大写字母
                //必须包含特殊符号
                //至少6个字符，最多16个字符
                var regex = new Regex(@"
                                      (?=.*[0-9])                     
                                      (?=.*[a-zA-Z])                 
                                      (?=([\x21-\x7e]+)[^a-zA-Z0-9])  
                                      .{6,16}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                if (!regex.IsMatch(userDto.Password))
                {
                    return new ValidationResult("密码不符合规则,请重新输入", new[] { nameof(userDto.Password) });
                }
            }
            if (!string.IsNullOrWhiteSpace(userDto.ConfirmPassword))
            {
                if (!userDto.Password.Equals(userDto.ConfirmPassword))
                {
                    return new ValidationResult("两次输入密码不同,请重新输入", new[] {
                        nameof(userDto.Password),
                        nameof(userDto.ConfirmPassword)
                    });
                }
            }
            return ValidationResult.Success;
        }

    }


}
