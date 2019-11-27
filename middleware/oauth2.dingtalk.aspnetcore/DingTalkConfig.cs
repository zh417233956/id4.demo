using oauth2.dingtalk.aspnetcore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore
{
    /// <summary>
    /// 配置
    /// 
    /// 步骤：authorize => user
    /// </summary>
    public class DingTalkConfig
    {
        #region API请求接口

        // 扫码模式

        /// <summary>
        /// GET
        /// </summary>
        public string API_Authorize_ScanCode = "https://oapi.dingtalk.com/connect/qrconnect";

        // 密码模式

        /// <summary>
        /// GET
        /// </summary>
        public string API_Authorize_Password = "https://oapi.dingtalk.com/connect/oauth2/sns_authorize";

        /// <summary>
        /// GET
        /// </summary>
        public string API_AccessToken = "https://oapi.dingtalk.com/sns/gettoken";

        /// <summary>
        /// POST
        /// </summary>
        public string API_User = "https://oapi.dingtalk.com/sns/getuserinfo_bycode";

        #endregion

        /// <summary>
        /// appId
        /// </summary>
        public string appId = "";

        /// <summary>
        /// appSecret
        /// </summary>
        public string appSecret = "";

        /// <summary>
        /// 回调
        /// </summary>
        public Func<string> Redirect_Uri { get; set; }

        public ISet<string> Scopes { get; } = new HashSet<string> { "snsapi_login" };

        public bool IsMobile { get; }

        public DingTalkConfig(string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            appId = clientId;

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }
            appSecret = clientSecret;
        }


        public string BuildAuthorizationUrl(string state)
        {
            var scopes = new HashSet<string>()
                .AddAll(Scopes);

            var scope = string.Join(",", scopes);

            var authorizationUrl = string.Format("{0}?response_type=code&appid={1}&redirect_uri={2}&state={3}&scope={4}",
                API_Authorize_ScanCode, Uri.EscapeDataString(appId), 
                Uri.EscapeDataString(Redirect_Uri()), 
                Uri.EscapeDataString(state), 
                Uri.EscapeDataString(scope));

            return authorizationUrl;
        }

        public string BuildOpenIdUrl()
        {
            var entity = new Response.DingTalk_User_RequestEntity(appId, appSecret);
            return string.Format("{0}?{1}", API_User, entity.EntityToPars());
        }
    }
}
