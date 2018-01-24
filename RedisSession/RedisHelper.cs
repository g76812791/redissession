using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace RedisSession
{
    public class RedisHelper
    {
        private RedisClient Redis = new RedisClient("192.168.107.232");
        //缓存池
        PooledRedisClientManager prcm = new PooledRedisClientManager();

        //默认缓存过期时间单位秒
        public int secondsTimeOut = 20 * 60;

        /// <summary>
        /// 缓冲池
        /// </summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        /// <returns></returns>
        public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts,
                new RedisClientManagerConfig
                {
                    MaxWritePoolSize = readWriteHosts.Length * 5,
                    MaxReadPoolSize = readOnlyHosts.Length * 5,
                    AutoStart = true,
                });
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OpenPooledRedis">是否开启缓冲池</param>
        public RedisHelper(bool OpenPooledRedis = false)
        {

            if (OpenPooledRedis)
            {
                prcm = CreateManager(new string[] { "192.168.107.232:6379" }, new string[] { "192.168.107.232:6379" });
                Redis = prcm.GetClient() as RedisClient;
            }
        }
        /// <summary>
        /// 距离过期时间还有多少秒
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long TTL(string key)
        {
            //            return Redis.GetTimeToLive(key).TotalSeconds;

            return Redis.Ttl(key);
        }
        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        public void Expire(string key, int timeout = 0)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(key, secondsTimeOut);
            }
        }


        public bool SetValue<T>(string key, T value, TimeSpan timespan)
        {
            if (typeof(T) == typeof(string))
            {
                try
                {
                    Redis.SetEntry(key, value as string, timespan);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return Redis.Set<T>(key, value, timespan);
        }

        public void SetValue<T>(string key, T value)
        {
            if (typeof(T) == typeof(string))
            {
                try
                {
                    Redis.SetEntry(key, value as string);
                }
                catch
                {
                }
            }
            Redis.Set<T>(key, value);
        }

        public T GetValue<T>(string key)
        {
            return Redis.Get<T>(key);
        }
        public long Delete(string key)
        {
            return Redis.Del(key);
        }
    }
}