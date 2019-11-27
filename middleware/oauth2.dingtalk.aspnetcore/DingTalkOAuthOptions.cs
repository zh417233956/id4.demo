using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore
{
    public class DingTalkOAuthOptions : OAuthOptions
    {
        public DingTalkOAuthOptions()
        {
            base.AuthorizationEndpoint = "empty";
            base.TokenEndpoint = "empty";
            base.CallbackPath = new PathString("/oauth2/dingtalk/callback");
            base.Scope.Add("snsapi_login");
        }

        public bool IsMobile { get; set; } = false;

        internal DingTalkConfig BuildDingTalkOptions(Func<string> redirectUrl)
        {
            return new DingTalkConfig(ClientId, ClientSecret)
            {
                Redirect_Uri = redirectUrl
            };
        }
    }
}