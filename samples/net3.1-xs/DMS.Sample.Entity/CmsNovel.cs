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
        /// 小说类型 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "type")]
        public int Type { get; set; }
        /// <summary>
        /// 小说类型名称 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "type_name")]
        public string TypeName { get; set; }
        /// <summary>
        /// 小说名称 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 作者 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "author")]
        public string Author { get; set; }
        /// <summary>
        /// 图片路径 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "logourl")]
        public string Logourl { get; set; }
        /// <summary>
        /// 是否完本，0默认连载，1已完本 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "full_flag")]
        public byte FullFlag { get; set; }
        /// <summary>
        /// 标题 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// 关键词 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "keywords")]
        public string Keywords { get; set; }
        /// <summary>
        /// 描述 
        ///</summary>
        [SugarColumn(ColumnName = "description")]
        public string Description { get; set; }
        /// <summary>
        /// 推荐类型，0不推荐，1首页推荐 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "recommend_type")]
        public int RecommendType { get; set; }
        /// <summary>
        /// 创建时间 
        /// 默认值: CURRENT_TIMESTAMP
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
