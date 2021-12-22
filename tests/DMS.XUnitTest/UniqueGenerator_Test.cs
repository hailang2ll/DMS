using DMS.UniqueGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DMS.XUnitTest
{
    public class UniqueGenerator_Test
    {
        /// <summary>
        /// 消息日志测试
        /// </summary>
        [Fact]
        public void GetWorkerID()
        {
            var id = UniqueHelper.GetWorkerID();
        }
    }
}
