using DMS.Sample31.Contracts;

namespace DMS.Sample31.Service
{
    public class ProductService : IProductService
    {
        public int Add()
        {
            UserEntity entity = new UserEntity()
            {
                UserID = 1125964271981826048,
                UserName = "aaaa",
                Pwd = "pwd"
            };

            return 300;
        }
    }
}
