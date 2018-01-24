using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using ServiceStack.Redis;

namespace RedisSession
{
    public class SessionHelper<T>
    {
        private const int secondsTimeOut = 60 * 20; //默认过期时间20分钟  单位秒
        public RedisHelper Redisc = new RedisHelper(false);
        public T this[string key, string sourceid]
        {
            get
            {
                string webCookie = CookieHelp.GetCookieValByKey(key);
                if (webCookie == "")
                {
                    return default(T);
                }

                //距离过期时间还有多少秒
                var l = Redisc.TTL(key);
                if (l >= 0)
                {
                    Redisc.Expire(key, secondsTimeOut);
                }
                return Redisc.GetValue<T>(key);
            }
            set
            {
                SetSession(key, sourceid, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">webcookie标识</param>
        /// <param name="sourceid">加密的sourceid获取redis缓存数据</param>
        /// <param name="value"></param>
        public bool SetSession(string key, string sourceid, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Key is Null or Epmty");
            }
            CookieHelp.SetCookies(key, sourceid);
            return Redisc.SetValue(key, value, TimeSpan.FromSeconds(secondsTimeOut));
        }
        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Remove(string key)
        {
            var rs = Redisc.Delete(key);
            CookieHelp.ClearCookieValByKey(key);
            return rs;
        }
    }
}



