﻿using DMSN.Common.JsonHandler;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Redis
{
    public class RedisRepository : IRedisRepository
    {
        private readonly ConnectionMultiplexer _conn;
        private readonly IDatabase _database;
        public RedisRepository(ConnectionMultiplexer conn)
        {
            _conn = conn;
            _database = _conn.GetDatabase();
        }

        private IServer GetServer()
        {
            var endpoint = _conn.GetEndPoints();
            return _conn.GetServer(endpoint.First());
        }


        public async Task<bool> Set(string key, object value, TimeSpan? expiry = null)
        {
            if (value is string cacheValue)
            {
                return await _database.StringSetAsync(key, cacheValue, expiry);
            }
            else
            {
                return await _database.StringSetAsync(key, value.SerializeObject(), expiry);
            }
        }
        public async Task<bool> Set(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToList();
            return await _database.StringSetAsync(newkeyValues.ToArray());
        }
        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public async Task<string> GetValue(string key)
        {
            return await _database.StringGetAsync(key);
        }
        public async Task<T> GetValue<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonSerializerExtensions.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }
        public async Task<RedisValue[]> GetValue(List<string> listKey)
        {
            List<string> newKeys = listKey.ToList();
            var keys = newKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
            return await _database.StringGetAsync(keys);
        }
        public async Task<bool> Exist(string key)
        {
            return await _database.KeyExistsAsync(key);
        }
        public async Task<bool> Remove(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }
        public async Task Clear()
        {
            foreach (var endPoint in _conn.GetEndPoints())
            {
                var server = GetServer();
                foreach (var key in server.Keys())
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }


        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            return await _database.HashExistsAsync(key, dataKey);
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            return await _database.HashSetAsync(key, dataKey, t.SerializeObject());
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            return await _database.HashDeleteAsync(key, dataKey);
        }
        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys)
        {
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await _database.HashDeleteAsync(key, dataKeys.ToArray());
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGeAsync<T>(string key, string dataKey)
        {
            string value = await _database.HashGetAsync(key, dataKey);
            return value.DeserializeObject<T>();
        }
        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            return await _database.HashIncrementAsync(key, dataKey, val);
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            return await _database.HashDecrementAsync(key, dataKey, val);
        }
        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashKeysAsync<T>(string key)
        {
            RedisValue[] values = await _database.HashKeysAsync(key);
            return ConvetList<T>(values);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> HashValuesAsync<T>(string key)
        {
            var result = new List<T>();
            HashEntry[] arr = await _database.HashGetAllAsync(key);
            foreach (var item in arr)
            {
                string values = item.Name;
                if (!item.Value.IsNullOrEmpty)
                {
                    var val = JsonSerializerExtensions.DeserializeObject<T>(item.Value);
                    result.Add(val);
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<HashEntry[]> HashValueAllAsync(string key)
        {
            HashEntry[] arr = await _database.HashGetAllAsync(key);
            return arr;
        }


        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            string result = value is string ? value.ToString() : value.SerializeObject();
            return await _database.ListRemoveAsync(key, result);
        }
        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        {
            var values = await _database.ListRangeAsync(key);
            return ConvetList<T>(values);
        }
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            return await _database.ListRightPushAsync(key, ConvertJson(value));
        }
        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        {
            string value = await _database.ListRightPopAsync(key);
            return value.DeserializeObject<T>();
        }
        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            return await _database.ListLeftPushAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            string value = await _database.ListLeftPopAsync(key);
            return value.DeserializeObject<T>();
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            return await _database.ListLengthAsync(key);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            return await _database.SortedSetAddAsync(key, ConvertJson<T>(value), score);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            return await _database.SortedSetRemoveAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            var values = await _database.SortedSetRangeByRankAsync(key);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            return await _database.SortedSetLengthAsync(key);
        }




        #region private

        private string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : value.SerializeObject();
            return result;
        }
        private List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = JsonConvert.DeserializeObject<T>(item);
                result.Add(model);
            }
            return result;
        }
        #endregion

    }
}