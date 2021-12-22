using DMS.Common.JsonHandler;
using System;
using Xunit;

namespace DMS.XUnitTest
{

    /// <summary>
    /// 用户实体
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 用户时间
        /// </summary>
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonSerializerExtensions_Test
    {
        /// <summary>
        /// 序列化/反序列化
        /// ConfigureServices=>AddJsonOptions{ options.SerializerSettings.Converters.Add(new CustomStringConverter());}
        /// [ColumnMapping(Name = "ShopMemberID"), JsonConverter(typeof(CustomStringConverter))]
        /// </summary>
        [Fact]
        public void SerializerExtensions()
        {
            UserEntity user = new UserEntity()
            {
                UserID = 1125964271981826048,
                UserName = "aaaa",
                Pwd = "pwd"
            };
            string serObject = user.SerializeObject();
            UserEntity u = serObject.DeserializeObject<UserEntity>();

            string s = "aaaaaaaaaaa";
            var ss = s.SerializeObject();
            var bb = ss.DeserializeObject<string>();

        }
    }
}
