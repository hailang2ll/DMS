using DMSN.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.UniqueGenerator
{
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
}
