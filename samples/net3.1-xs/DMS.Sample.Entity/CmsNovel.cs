using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace DMS.Sample.Entity
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("cms_novel")]
    public class CmsNovel
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        /// <summary>
        /// 标题 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// 内容 
        ///</summary>
        [SugarColumn(ColumnName = "content")]
        public string Content { get; set; }
        /// <summary>
        /// 创建时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
