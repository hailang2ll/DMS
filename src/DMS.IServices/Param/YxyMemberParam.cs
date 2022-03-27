using DMS.Common.Model.Param;

namespace DMS.IServices.Param
{
    /// <summary>
    /// 添加用户
    /// </summary>
    public class YxyMemberParam
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string MemberName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchYxyMemberParam : PageParam
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string MemberName { get; set; }

    }
}
