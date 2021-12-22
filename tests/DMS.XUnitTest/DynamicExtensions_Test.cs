using DMS.Common.Extensions;
using System.Collections.Generic;
using Xunit;

namespace DMS.XUnitTest
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DynamicExtensions_Test
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact(DisplayName = "")]
        public void Test1()
        {
            IEnumerable<dynamic> users = new List<User>() { new User() { Id = 1, Name = "a", Age = 1 } };
            //List<User> users = new List<User>();
            //users.Add(new User() { Id = 1, Name = "a", Age = 1 });
            //users.Add(new User() { Id = 2, Name = "a", Age = 2 });
            //users.Add(new User() { Id = 3, Name = "a", Age = 3 });
            //users.Add(new User() { Id = 4, Name = "a", Age = 4 });

            User user = null;
            foreach (var item in users)
            {
                CreateModel(item, ref user);
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        public void CreateModel(dynamic entity, ref User user)
        {
            var t = DynamicExtensions.GetDynamicValue(entity, "Id", 333);
            user = new User()
            {
                Id = DynamicExtensions.GetDynamicValue(entity, "Id", 333),
                //Age = entity.GetType().GetProperty("a") != null ? entity.Age : 2,
                //Name = DynamicExtensions.IsPropertyExist(entity, "b") ? entity.Age : "ccc",
                Name = DynamicExtensions.GetDynamicValue(entity, "f", "fff"),
            };
        }
    }
}
