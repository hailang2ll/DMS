using DMS.UniqueGenerator.Snowflake;

namespace DMS.UniqueGenerator
{
    /// <summary>
    /// UID生成
    /// </summary>
    public class UIDGenerator
    {
        //大并发的情况下，减少new的次数可以有效避免重复的可能
        private static IdWorker worker = new IdWorker(1, 1);
        /// <summary>
        /// 生成全局唯一ID
        /// </summary>
        /// <returns></returns>
        public static long GetWorkerID()
        {
            long id = worker.NextId();
            return id;
        }
    }
}
