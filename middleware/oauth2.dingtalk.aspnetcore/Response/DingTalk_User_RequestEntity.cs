using oauth2.dingtalk.aspnetcore.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace oauth2.dingtalk.aspnetcore.Response
{
    //
    // 摘要:
    //     /// user ///
    public class DingTalk_User_RequestEntity
    {
        //
        // 摘要:
        //     /// appid ///
        public string accessKey
        {
            get;
            set;
        }


        //
        // 摘要:
        //     /// 当前时间戳，单位是毫秒 ///
        public string timestamp
        {
            get;
            set;
        } = DateTime.Now.ToTimestamp().ToString() + "000";


        //
        // 摘要:
        //     /// 通过appSecret计算出来的签名值 ///
        public string signature
        {
            get;
            set;
        }

        //
        // 摘要:
        //     /// 构造，签名 ///
        public DingTalk_User_RequestEntity(string appid,string appSecret)
        {
            accessKey = appid;
            signature = HMAC_SHA256(timestamp, appSecret);
        }

        string HMAC_SHA256(string str, string key)
        {
            HMACSHA256 hMACSHA = new HMACSHA256
            {
                Key = Encoding.UTF8.GetBytes(key)
            };
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] inArray = hMACSHA.ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }
    }
}
