using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Sample.Contracts
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 用户时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
