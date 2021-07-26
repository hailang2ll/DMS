namespace DMSF.Common.BaseResult
{
    public class DataResultBase
    {
        /// <summary>
        /// 状态，0=成功，非0失败
        /// </summary>
        public int? errno { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 扩展字段1
        /// </summary>
        public object Ext1 { get; set; }

        public DataResultBase()
        {
            errno = 0;//默认为0
            errmsg = "";
            Ext1 = new object();
        }
    }
}
