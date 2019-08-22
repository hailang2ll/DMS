namespace DMS.Common.BaseParam
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageParam
    {
        private int _pageIndex = 1;
        public PageParam(int pageSize = 16)
        {
            this.PageSize = pageSize;
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value <= 0 ? 1 : value; }
        }
        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 多少页
        /// </summary>
        public int? TotalPage { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int? SortMode { get; set; }

    }
}
