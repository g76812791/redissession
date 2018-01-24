using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Data;
using System.Xml;
using System.Web;
using System.Collections;
using System.Net.Cache;
namespace RedisSession
{
    public static class CookieHelp
    {
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="keystr">Cookies名字</param>
        /// <param name="values">Cookies值</param>
        /// <param name="timeout">过期时间</param>
        /// <param name="CookieDoamin">作用域</param>
        public static void SetCookies(string keystr, string values, DateTime timeout )
        {
            HttpResponse Response = HttpContext.Current.Response;
            Response.Cookies[keystr].Value = AES.UrlEncrypt(values);
            Response.Cookies[keystr].Expires = timeout;
            Response.Cookies[keystr].HttpOnly = true;
            //Response.Cookies[keystr].Domain = CookieDoamin;
        }
        public static void SetCookies(string keystr, string values)
        {
            HttpResponse Response = HttpContext.Current.Response;
            Response.Cookies[keystr].Value = AES.UrlEncrypt(values);
            Response.Cookies[keystr].HttpOnly = true;
        }

        //HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
        /// <summary>
        ///获取指定Cookie值
        /// </summary>
        /// <param name="cookies">Cookie集合</param>
        /// <param name="key">Cookie键值</param>
        /// <returns>Cookie值</returns>
        public static string GetCookieValByKey( string key)
        {
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
            return cookies[key] != null && !String.IsNullOrEmpty(cookies[key].Value) ? AES.UrlDecrypt(cookies[key].Value) : string.Empty;
        }

        public static void ClearCookieValByKey(string key)
        {
            HttpCookie objCookie = new HttpCookie(key.Trim());
            objCookie.Expires = DateTime.Now.AddYears(-5);
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

    }
}
