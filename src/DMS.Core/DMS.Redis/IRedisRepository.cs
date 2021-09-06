using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Redis
{
    public interface IRedisRepository
    {
        Task<bool> Set(string key, object value, TimeSpan? expiry = default(TimeSpan?));
        Task<bool> Set(List<KeyValuePair<RedisKey, RedisValue>> keyValues);
        Task<string> GetValue(string key);
        //获取值，并序列化
        Task<T> GetValue<T>(string key);
        Task<RedisValue[]> GetValue(List<string> listKey);
        //判断是否存在
        Task<bool> Exist(string key);
        //移除某一个缓存值
        Task<bool> Remove(string key);
        //全部清除
        Task Clear();

        Task<bool> HashExistsAsync(string key, string dataKey);
        Task<bool> HashSetAsync<T>(string key, string dataKey, T t);
        Task<bool> HashDeleteAsync(string key, string dataKey);
        Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys);
        Task<T> HashGeAsync<T>(string key, string dataKey);
        Task<double> HashIncrementAsync(string key, string dataKey, double val = 1);
        Task<double> HashDecrementAsync(string key, string dataKey, double val = 1);
        Task<List<T>> HashKeysAsync<T>(string key);
        Task<List<T>> HashValuesAsync<T>(string key);
        Task<HashEntry[]> HashValueAllAsync(string key);


        Task<long> ListRemoveAsync<T>(string key, T value);
        Task<List<T>> ListRangeAsync<T>(string key);
        Task<long> ListRightPushAsync<T>(string key, T value);
        Task<T> ListRightPopAsync<T>(string key);
        Task<long> ListLeftPushAsync<T>(string key, T value);
        Task<T> ListLeftPopAsync<T>(string key);
        Task<long> ListLengthAsync(string key);


        Task<bool> SortedSetAddAsync<T>(string key, T value, double score);
        Task<bool> SortedSetRemoveAsync<T>(string key, T value);
        Task<List<T>> SortedSetRangeByRankAsync<T>(string key);
        Task<long> SortedSetLengthAsync(string key);
    }
}
