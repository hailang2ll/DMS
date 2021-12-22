using DMS.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS.UniqueGenerator
{
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
}
