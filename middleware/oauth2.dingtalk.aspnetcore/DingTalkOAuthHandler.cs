using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace oauth2.dingtalk.aspnetcore
{
    public class DingTalkOAuthHandler : OAuthHandler<DingTalkOAuthOptions>
    {
        public DingTalkOAuthHandler(
            IOptionsMonitor<DingTalkOAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        private DingTalkClient _innerClient;

        private DingTalkClient InnerClient
        {
            get
            {
                if (_innerClient == null)
                {
                    var qqConnectOptions = Options.BuildDingTalkOptions(GetRedirectUrl);
                    _innerClient = new DingTalkClient(Backchannel, qqConnectOptions);
                }
                return _innerClient;
            }
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <returns></returns>
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            try
            {
                var code = Request.Query["code"].ToString();
                var state = Request.Query["state"].ToString();

                if (code == null)
                {
                    return HandleRequestResult.Fail("Code was not found.");
                }

                var properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return HandleRequestResult.Fail("The oauth state was missing or invalid.");
                }

                var openId = await InnerClient.GetOpenId(
                    code,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(openId.unionid))
                {
                    return HandleRequestResult.Fail("unionid was not found.");
                }

                var qqConnectProfile = DingTalkConnectProfile.From(Scheme.Name, Options.ClientId, openId);

                var principal = qqConnectProfile.BuildClaimsPrincipal();

                var ticket = new AuthenticationTicket(principal, properties, Scheme.Name);

                return HandleRequestResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return HandleRequestResult.Fail(ex);
            }
        }
        /// <summary>
        /// 请求地址
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {           

            var state = Options.StateDataFormat.Protect(properties);
            return InnerClient.BuildAuthorizationUrl(state);
        }
        /// <summary>
        /// 回调地址
        /// </summary>
        /// <returns></returns>
        private string GetRedirectUrl()
        {
            return Request.Scheme
                + "://"
                + Request.Host
                + OriginalPathBase
                + Options.CallbackPath;
        }
    }
}