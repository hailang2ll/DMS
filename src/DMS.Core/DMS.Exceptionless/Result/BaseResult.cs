namespace DMS.Exceptionless.Result
{
    /// <summary>
    /// 返回值
    /// </summary>
    class BaseResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int errno { get; set; }

        /// <summary>
        /// 错误文案
        /// </summary>
        public string errmsg { get; set; }
    }
}