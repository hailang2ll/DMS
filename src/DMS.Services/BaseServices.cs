using DMS.IServices;
using DMS.Repository;
using SqlSugar;

namespace DMS.Services
{
    public class BaseService<T> : BaseRepository<T> where T : class, new()
    { }

}
