using DMS.Common.JsonHandler;
using DMS.Common.XmlHandler;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DMS.Common.Test
{
    public class StaticXmlHandler_Test
    {
        /// <summary>
        /// 
        /// </summary>
        [Test(Description = "从XML中读取地址信息")]
        public void Test1()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "SiteXml_Sys_AddressInfoExt.xml");
            List<Sys_AddressInfoExt> addressList = new StaticXmlHandler<Sys_AddressInfoExt>(filePath).ResultList;

            var strjson = JsonSerializerExtensions.SerializeObject(addressList);
            Assert.Pass();
        }


        [Test(Description = "从数据库中读取地址信息")]
        public void GetAddressList()
        {
            //List<AddressEntity> addressList = DMS.Create<Sys_Address>().ToList<AddressEntity>();
            //string jsonStr = GetLocalJson(100000, addressList);

            Assert.Pass();
        }
        private string GetLocalJson(int pid, List<AddressEntity> source)
        {
            List<Province> provinceList = new List<Province>();

            List<AddressEntity> fristList = source.Where(a => a.ParentId == pid).ToList();
            foreach (AddressEntity item in fristList)
            {
                Province province = new Province()
                {
                    ID = item.ID,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    LevelType = item.LevelType,
                    CityCode = item.CityCode,
                    ZipCode = item.ZipCode,
                };

                var cityList = source.Where(q => q.ParentId == item.ID).ToList();
                foreach (var cityItem in cityList)
                {
                    City cityEntity = new City()
                    {
                        ID = cityItem.ID,
                        Name = cityItem.Name,
                        ParentId = cityItem.ParentId,
                        LevelType = cityItem.LevelType,
                        CityCode = cityItem.CityCode,
                        ZipCode = cityItem.ZipCode,
                    };

                    //区
                    var areaList = source.Where(m => m.ParentId == cityItem.ID).ToList();
                    foreach (var areaItem in areaList)
                    {
                        AddressEntity areaEntity = new AddressEntity()
                        {
                            ID = areaItem.ID,
                            Name = areaItem.Name,
                            ParentId = areaItem.ParentId,
                            LevelType = areaItem.LevelType,
                            CityCode = areaItem.CityCode,
                            ZipCode = areaItem.ZipCode,
                        };
                        cityEntity.Areas.Add(areaEntity);
                    }

                    province.City.Add(cityEntity);
                }

                provinceList.Add(province);
            }

            string json = JsonConvert.SerializeObject(provinceList);
            return json;
        }


    }



    public class Province : AddressEntity
    {
        public Province()
        {
            this.City = new List<City>();
        }
        public List<City> City { get; set; }

    }
    public class City : AddressEntity
    {
        public City()
        {
            this.Areas = new List<AddressEntity>();
        }
        public List<AddressEntity> Areas { get; set; }
    }
    public class AddressEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int LevelType { get; set; }
        public string CityCode { get; set; }
        public string ZipCode { get; set; }
    }




    public class Sys_AddressInfoExt : Sys_AddressInfo
    {
        public Sys_AddressInfoExt()
        {
            this.AddressList = new List<Sys_AddressInfo>();
        }
        public List<Sys_AddressInfo> AddressList { get; set; }
    }
    public class Sys_AddressInfo
    {

        #region Private Properties

        private int? _addressId;//
        private string _nameCode;//
        private string _addressName;//
        private string _codePath;//
        private string _namePath;//
        private int? _levelPath;//
        private int? _parentId;//
        private string _areaCode;//
        private string _zipCode;//

        #endregion

        #region Public Properties

        /// <summary>
        /// .
        /// </summary>
        public int? AddressId
        {
            get { return _addressId; }
            set { _addressId = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public string NameCode
        {
            get { return _nameCode; }
            set { _nameCode = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public string AddressName
        {
            get { return _addressName; }
            set { _addressName = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public string CodePath
        {
            get { return _codePath; }
            set { _codePath = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public string NamePath
        {
            get { return _namePath; }
            set { _namePath = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public int? LevelPath
        {
            get { return _levelPath; }
            set { _levelPath = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public int? ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public string AreaCode
        {
            get { return _areaCode; }
            set { _areaCode = value; }
        }

        /// <summary>
        /// .
        /// </summary>
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        #endregion

    }

}
