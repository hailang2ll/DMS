using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace DMS.Sample.Entity
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("cms_user")]
    public class CmsUser
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        /// <summary>
        /// 用户名 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "user_password")]
        public string UserPassword { get; set; }
        /// <summary>
        /// 创建时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
