using DMS.IServices.Param;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DMS.IServices.Validation.Fluent
{
    /// <summary>
    /// FluentValidation模型验证
    /// </summary>
    public class AddMemberParamValidator : AbstractValidator<AddMemberParam>
    {
        public AddMemberParamValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.MemberName).NotEmpty().WithMessage("用户名不能为空").Length(2, 12).WithMessage("用户名至少2个字符，最多12个字符");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空").Length(6, 16).WithMessage("密码长度至少6个字符，最多16个字符")
                .Must(EncryptionPassword).WithMessage("密码不符合规则,必须包含数字、小写或大写字母、特殊符号");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("确认密码不能为空").Must(ComparePassword).WithMessage("确认密码必须跟密码一样");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("手机号不能为空").Must(IsMobile).WithMessage("手机号格式不正确");
        }

        private bool EncryptionPassword(string password)
        {
            //正则
            var regex = new Regex(@"
                                (?=.*[0-9]) 
                                (?=.*[a-zA-Z])  
                                (?=([\x21-\x7e]+)[^a-zA-Z0-9])  
                                .{6,16}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return regex.IsMatch(password);
        }

        /// 比较两次密码是否一样   
        /// 这里传的是：ConfirmPassword    
        private bool ComparePassword(AddMemberParam model, string confirmpwd)
        {
            return string.Equals(model.Password, confirmpwd, StringComparison.OrdinalIgnoreCase);
        }
        private bool IsMobile(string arg)
        {
            return Regex.IsMatch(arg, @"^[1][3-8]\d{9}$");
        }
    }

}
