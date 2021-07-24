namespace DMS.Common.BaseResult
{
    /// <summary>
    /// 请求响应结果
    /// </summary>
    public class ResponseResult : DataResultBase
    { /// <summary>
      /// 数据
      /// </summary>
        public object data { get; set; }

    }

    /// <summary>
    ///  请求响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> : DataResultBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }

    }

    /// <summary>
    ///  请求响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponsePageResult<T> : DataResultBase
    {
        public DataResultList<T> data { get; set; }
    }
}
