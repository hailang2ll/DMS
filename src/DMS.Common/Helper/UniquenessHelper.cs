using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.Common.Helper
{
    public class UniquenessHelper
    {
        /// <summary> 
        /// 生成GUID唯一字符串
        /// </summary> 
        /// <returns></returns> 
        public static string GetGuid()
        {
            System.Guid g = System.Guid.NewGuid();
            return g.ToString();
        }

        /// <summary>  
        /// 根据GUID获取16位的唯一字符串
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>
        /// <returns></returns>
        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        private static IdWorker worker = new IdWorker(1, 1);
        /// <summary>
        /// 19位雪花算法，生成全局唯一
        /// </summary>
        /// <returns></returns>
        public static long GetWorkerID()
        {
            long id = worker.NextId();
            return id;
        }



        #region OrderNumberGenerator
        public class OrderNumberGenerator
        {
            static readonly object readLockerChannel = new object();
            static readonly object readLockerMaster = new object();
            static string rootPath = "";//AppSettings.GetConfigPath;
            static string app = null;//System.Configuration.ConfigurationManager.AppSettings["OrderNumApp"];

            static Dictionary<string, List<string>> orderNumList = new Dictionary<string, List<string>>();

            static OrderNumberGenerator()
            {
                if (app == null)
                {
                    string appFileName = System.IO.Path.Combine(rootPath, "OrderNumApp.config");
                    using (System.IO.FileStream steam = new System.IO.FileStream(appFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(steam))
                        {
                            app = reader.ReadToEnd().Trim();
                        }
                    }
                }
            }

            public static void Clear(DateTime date)
            {
                string masterKey = "11" + "12" + app + date.ToString("yyMMdd");
                string channelOrderKey = "21" + "12" + app + date.ToString("yyMMdd");
                if (orderNumList.ContainsKey(masterKey))
                {
                    orderNumList.Remove(masterKey);
                }
                if (orderNumList.ContainsKey(channelOrderKey))
                {
                    orderNumList.Remove(channelOrderKey);
                }

            }

            //public static void CreateMasterOrderNum(DateTime date, int count)
            //{
            //    string webSiteType = "11";//主站
            //    string consumeType = "12";//购买

            //    CreateOrderNum(webSiteType, consumeType, date, count);
            //}

            //public static void CreateChannelOrderNum(DateTime date, int count)
            //{
            //    string webSiteType = "21";//渠道
            //    string consumeType = "12";//购买

            //    CreateOrderNum(webSiteType, consumeType, date, count);
            //}

            private static void CreateOrderNum(string webSiteType, string consumeType, DateTime date, int count)
            {
                List<string> numList = null;
                List<string> usedList = new List<string>();
                string preNum = webSiteType + consumeType + app + date.ToString("yyMMdd");
                if (orderNumList.ContainsKey(preNum))
                {
                    numList = orderNumList[preNum];
                    if (numList.Count >= count)
                    {
                        return;
                    }
                }
                else
                {
                    numList = new List<string>();
                    orderNumList.Add(preNum, numList);
                }
                string filePath = System.IO.Path.Combine(rootPath, String.Format("{0}\\{1}", "OrderNum", date.ToString("yyMM")));
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string fileName = System.IO.Path.Combine(filePath, preNum + ".dat");
                using (System.IO.FileStream steam = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(steam))
                    {
                        usedList = reader.ReadToEnd().Split(',').ToList();
                    }
                }
                while (numList.Count < count)
                {
                    string orderNum = preNum + RandomHelper.CreateRandom(100000, 999999);
                    if (numList.Exists(c => c == orderNum) || usedList.Exists(c => c == orderNum))
                    {
                        continue;
                    }
                    numList.Add(orderNum);
                }
            }

            public static string GetMasterOrderNum()
            {
                string num = "";
                DateTime date = DateTime.Now;
                string webSiteType = "11";//主站
                string consumeType = "12";//购买
                lock (readLockerMaster)
                {
                    num = GetOrderNum(webSiteType, consumeType, date);

                }
                return num;
            }

            public static string GetChannelOrderNum()
            {
                string num = "";
                DateTime date = DateTime.Now;
                string webSiteType = "21";//渠道
                string consumeType = "12";//购买
                lock (readLockerChannel)
                {
                    num = GetOrderNum(webSiteType, consumeType, date);

                }
                return num;
            }

            private static string GetOrderNum(string webSiteType, string consumeType, DateTime date)
            {
                string num = "";
                string preNum = webSiteType + consumeType + app + date.ToString("yyMMdd");
                if (!orderNumList.ContainsKey(preNum) || orderNumList[preNum].Count == 0)
                {
                    CreateOrderNum(webSiteType, consumeType, date, 500);
                }
                List<string> numList = orderNumList[preNum];
                num = numList[0];
                string filePath = System.IO.Path.Combine(rootPath, String.Format("{0}\\{1}", "OrderNum", date.ToString("yyMM")));
                string fileName = System.IO.Path.Combine(filePath, preNum + ".dat");
                using (System.IO.FileStream steam = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None))
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(steam))
                    {
                        if (steam.Length > 0)
                        {
                            writer.Write("," + num);
                        }
                        else
                        {
                            writer.Write(num);
                        }
                    }
                }
                numList.RemoveAt(0);

                return num;
            }
        }
        #endregion

        #region InsureNumberGenerator
        public class InsureNumberGenerator
        {
            static readonly object readLocker = new object();
            static string rootPath = "";// AppSettings.GetConfigPath;
            static string[] app = null;//System.Configuration.ConfigurationManager.AppSettings["InsureNumApp"].Split(',');

            static Dictionary<string, List<string>> insureNumList = new Dictionary<string, List<string>>();

            public static void Clear(DateTime date)
            {
                string key = date.ToString("yyMMdd");

                if (insureNumList.ContainsKey(key))
                {
                    insureNumList.Remove(key);
                }

            }

            public static void CreateInsureNum(DateTime date, int count)
            {
                if (app == null)
                {
                    string appFileName = System.IO.Path.Combine(rootPath, "InsureNumApp.config");//HzIns.BasicFramework.Common.Utility.GetFilePath("\\Config\\" + "InsureNumApp.config");
                    using (System.IO.FileStream steam = new System.IO.FileStream(appFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(steam))
                        {
                            app = reader.ReadToEnd().Trim().Split(',');
                        }
                    }
                }
                List<string> numList = null;
                List<string> usedList = new List<string>();
                string preNum = date.ToString("yyMMdd");// +app[DateTime.Now.Second % app.Length];
                if (insureNumList.ContainsKey(preNum))
                {
                    numList = insureNumList[preNum];
                    if (numList.Count >= count)
                    {
                        return;
                    }
                }
                else
                {
                    numList = new List<string>();
                    insureNumList.Add(preNum, numList);
                }
                string filePath = System.IO.Path.Combine(rootPath, String.Format("{0}\\{1}", "InsureNum", date.ToString("yyMM")));//HzIns.BasicFramework.Common.Utility.GetFilePath(String.Format("\\Data\\{0}\\{1}", "InsureNum", date.ToString("yyMM")));
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string fileName = System.IO.Path.Combine(filePath, preNum + ".dat");
                using (System.IO.FileStream steam = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(steam))
                    {
                        usedList = reader.ReadToEnd().Split(',').ToList();
                    }
                }
                while (numList.Count < count)
                {
                    string insureNum = preNum + app[DateTime.Now.Second % app.Length] + RandomHelper.CreateRandom(1000000, 9999999);
                    if (numList.Exists(c => c == insureNum) || usedList.Exists(c => c == insureNum))
                    {
                        continue;
                    }
                    numList.Add(insureNum);
                }
            }

            public static string GetInsureNum()
            {
                string num = "";
                DateTime date = DateTime.Now;
                lock (readLocker)
                {
                    num = GetInsureNum(date);

                }
                return num;
            }

            private static string GetInsureNum(DateTime date)
            {
                string num = "";
                string preNum = date.ToString("yyMMdd");// + app[DateTime.Now.Second % app.Length];
                if (!insureNumList.ContainsKey(preNum) || insureNumList[preNum].Count == 0)
                {
                    CreateInsureNum(date, 1000);
                }
                List<string> numList = insureNumList[preNum];
                num = numList[0];
                string filePath = System.IO.Path.Combine(rootPath, String.Format("{0}\\{1}", "InsureNum", date.ToString("yyMM")));
                string fileName = System.IO.Path.Combine(filePath, preNum + ".dat");
                using (System.IO.FileStream steam = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None))
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(steam))
                    {
                        if (steam.Length > 0)
                        {
                            writer.Write("," + num);
                        }
                        else
                        {
                            writer.Write(num);
                        }
                    }
                }
                numList.RemoveAt(0);

                return num;
            }
        }
        #endregion
    }
}
