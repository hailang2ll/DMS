using DMS.Common.XmlHandler;
using DMS.XUnitTest.Models.xml;
using System;
using System.IO;
using Xunit;

namespace DMS.XUnitTest
{
    public class XmlSerializerExtensions_Test
    {
        [Fact(DisplayName = "将XML转换成JSON")]
        public void Test1()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "gld.xml");
            string xml;
            using (var inputFileStream = File.OpenRead(filePath))
            {
                using (var inputStreamReader = new StreamReader(inputFileStream))
                {
                    xml = inputStreamReader.ReadToEnd();
                }
            }
            var json = XmlSerializerExtensions.XmlToJson(xml, "//上海市工程量清单电子文件");

        }

        [Fact(DisplayName = "将XML文件转换成实体")]
        public void XmlToModel_Test()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "gld.xml");
            var resQuery = XmlSerializerExtensions.XmlToModel<GldEntity>(filePath);
        }
        [Fact(DisplayName = "将XML文件转换成实体，指定节点")]
        public void XmlToModelByNode_Test()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "gld.xml");

            var result = XmlSerializerExtensions.XmlToModel<工程量清单>(filePath, "//上海市工程量清单电子文件//正文//工程量清单");
        }

        [Fact(DisplayName = "将XML流转换成实体")]
        public void XmlStream_Test()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "gld.xml");
            var stream = new FileStream(filePath, FileMode.Open);
            var result = XmlSerializerExtensions.XmlToModel<GldEntity>(stream);
        }
        [Fact(DisplayName = "将XML流转换成实体，指定节点")]
        public void XmlStreamByNode_Test()
        {
            var a = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "gld.xml");
            var stream = new FileStream(filePath, FileMode.Open);
            var result = XmlSerializerExtensions.XmlToModel<工程量清单>(stream, "//上海市工程量清单电子文件//正文//工程量清单");
        }
















        [Fact(DisplayName = "将XML转换成实体")]
        public void XmlTest_Test()
        {
            //string xml;
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "xmltest.xml");
            //using (var inputFileStream = File.OpenRead(filePath))
            //{
            //    using (var inputStreamReader = new StreamReader(inputFileStream))
            //    {
            //        xml = inputStreamReader.ReadToEnd();
            //    }
            //}
            //DocumentResult result = XmlSerializerExtensions.XmlToModel<DocumentResult>(xml);
        }
    }
}
