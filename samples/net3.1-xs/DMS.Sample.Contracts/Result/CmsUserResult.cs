using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Sample.Contracts.Result
{
    public class LoginCmsUserResult
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "id")]
        public long Id { get; set; }
        /// <summary>
        /// 用户名 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }
    }
}
