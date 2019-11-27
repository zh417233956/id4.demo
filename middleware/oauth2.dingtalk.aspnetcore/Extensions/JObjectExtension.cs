using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore.Extensions
{
    public static class JObjectExtension
    {
        public static string TryGetValue(this JObject @this, string name)
        {
            if (@this == null)
            {
                return null;
            }

            if (@this.TryGetValue(name, out var value))
            {
                return value.ToString();
            }

            return null;
        }
        /// <summary>
        /// 实体 转 Pars
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string EntityToPars<T>(this T entity)
        {
            string result = string.Empty;
            var pis = entity.GetType().GetProperties();
            foreach (var pi in pis)
            {
                string value = pi.GetValue(entity, null)?.ToString();
                if (value != null)
                {
                    result += "&" + pi.Name + "=" + value.ToEncode();
                }
            }
            return result.TrimStart('&');
        }

    }
}
