using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace oauth2.dingtalk.aspnetcore
{
    public static class ApplicationBuilderExtension
    {
        public static AuthenticationBuilder AddDingTalk(
            this AuthenticationBuilder @this,
            string scheme,
            string displayName,
            Action<DingTalkOAuthOptions> configureOptions)
        {
            return @this.AddOAuth<DingTalkOAuthOptions, DingTalkOAuthHandler>(scheme, displayName, configureOptions);
        }
    }
}
