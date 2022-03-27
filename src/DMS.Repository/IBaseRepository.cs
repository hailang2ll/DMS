using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Repository
{
    public interface IBaseRepository<T> : ISimpleClient<T> where T : class, new()
    {
        //可自定义扩展接口
    }
}
