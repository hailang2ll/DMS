using System;
using System.Security.Cryptography;
using System.Text;

namespace DMS.Common.Encrypt
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class MD5Helper
    {
        public static string GetPushURL(string url, string key, string streamName, DateTime dateTime)
        {
            //ResponseResult result = new ResponseResult();
            //string key = "7e9c110c0a0da51e33605fdcac2993db";
            //string streamName = sdkappid + "_" + MemberID;
            //string timeStamp = DMS.Common.Extensions.DateTimeExtensions.ToTimestamp(DateTime.Now.AddMinutes(3), false);
            //string txSecret = DMS.BaseFramework.Common.Encrypt.MD5Helper.MD5(key + streamName + timeStamp);
            //string url = $"rtmp://push.trydou.com/live/{streamName}?txSecret={txSecret}&txTime={timeStamp}";

            //result.data = url;
            //return await Task.FromResult(result);


            string timeStamp = DMS.Common.Extensions.DateTimeExtensions.ToTimestamp(dateTime, false);
            string txSecret = MD5(key + streamName + timeStamp);
            url = string.Format(url, streamName, txSecret, timeStamp);
            return url;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            byte[] array = Encoding.UTF8.GetBytes(str);
            array = new MD5CryptoServiceProvider().ComputeHash(array);
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("x").PadLeft(2, '0');
            }
            return text;

        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="Text">要加密的字符串</param>
        /// <returns>密文</returns>
        public static string MD5Crypto32(string Text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Text);
            System.Security.Cryptography.MD5CryptoServiceProvider md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = md5Provider.ComputeHash(buffer);
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString().ToUpper();
        }

        /// <summary>
        /// MD5加密3
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string MD5Pay(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            byte[] outputBye = m5.ComputeHash(inputBye);
            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }
}
