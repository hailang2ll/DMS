using DMS.Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Common.Test
{
    public class ConvertAll_Test
    {
        /// <summary>
        /// 
        /// </summary>
        [Test(Description = "")]
        public void StringConvertAll()
        {
            //List转字符串
            List<string> List = new List<string>();
            string strArray = string.Join(",", List);

            //字符串转List
            string str = "2,4,4,4";
            List = new List<string>(str.Split(','));

            //字符数组转Int数组
            int[] list = Array.ConvertAll<string, int>(str.Split(','), s => int.Parse(s));
            long[] cartIds = Array.ConvertAll<string, long>(str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), q => q.ToLong());
            string[] arr = str.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

            //List<string>字符串转Int数组
            List = new List<string>() { 
            "1",
            "2",
            "3",
            };
            strArray = string.Join(",", List);
            list = Array.ConvertAll<string, int>(strArray.Split(','), s => int.Parse(s));

            List<Guid?> ids = List.ConvertAll<Guid?>(q => { return q.ToGuid(); });
            //Guid?[] strategyKeys = Array.ConvertAll<string, Guid?>(param.ToArray(), item => TryParse.StrToGuid(item));
            //Array.ConvertAll<string, Guid?>(StrategyKeys.ToArray(), item => { return TryParse.StrToGuid(item); });


            var sNumbers = "1,2,3,4,5,6";
            List<int> numbers = sNumbers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();



        }
    }
}
