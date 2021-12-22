using DMS.Common.Helper;
using Xunit;

namespace DMS.XUnitTest
{

    /// <summary>
    /// 枚举扩展
    /// </summary>
    public class NumberHelper_Test
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Test()
        {
            var res = NumberHelper.Int2ChineseNum(1);
            res = NumberHelper.Int2ChineseNum(2);
            res = NumberHelper.Int2ChineseNum(3);
            res = NumberHelper.Int2ChineseNum(4);
            res = NumberHelper.Int2ChineseNum(5);
            res = NumberHelper.Int2ChineseNum(6);
            res = NumberHelper.Int2ChineseNum(7);
            res = NumberHelper.Int2ChineseNum(8);
            res = NumberHelper.Int2ChineseNum(9);
            res = NumberHelper.Int2ChineseNum(10);
        }
    }
}
