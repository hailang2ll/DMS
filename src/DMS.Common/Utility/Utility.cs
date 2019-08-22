using DMS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DMS.Common.Utility
{
    public class Utility
    {
        #region 字符串用Base64加密解密
        /// <summary>
        /// 字符串用Base64加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        public static string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(barray);
        }
        #endregion

        #region 汉字转换为Unicode编码
        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }

        /// <summary>  
        /// Unicode转字符串  
        /// </summary>  
        /// <param name="source">经过Unicode编码的字符串</param>  
        /// <returns>正常字符串</returns>  
        public static string ToUnicodeString(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        #endregion

        #region 对 URL 字符串进行编码
        /// <summary>
        /// 对 URL 字符串进行编码。
        /// </summary>
        /// <param name="s">要编码的文本。</param>
        /// <returns>一个已编码的字符串。</returns>
        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// 将已经为在 URL 中传输而编码的字符串转换为解码的字符串。
        /// </summary>
        /// <param name="s">要解码的字符串。</param>
        /// <returns>一个已解码的字符串。</returns>
        public static string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }
        #endregion

        #region 四舍五入
        /// <summary>
        /// 四舍五入 默认保留两位有效数字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static decimal Ex4s5R(decimal obj, int i)
        {
            return Math.Round(obj, i, MidpointRounding.AwayFromZero);
        }
        public static decimal Ex4s5R(decimal obj)
        {
            return Ex4s5R(obj, 2);
        }
        /// <summary>
        /// 只入不舍 默认保留两位有效数字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static decimal ExRu(decimal obj, int i)
        {
            int objs = obj.ToInt();
            if (obj - objs <= 0)
            {
                return objs;
            }

            string str = "0.";
            for (int j = 0; j < i; j++)
            {
                str += "0";
            }
            str += "5";
            decimal dec = Convert.ToDecimal(str);
            return Ex4s5R(obj + dec, i);
        }
        public static decimal ExRu(decimal obj)
        {
            return ExRu(obj, 2);
        }
        #endregion
    }
}
