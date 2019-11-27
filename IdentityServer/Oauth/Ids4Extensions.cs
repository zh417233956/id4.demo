using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using oauth2.dingtalk.aspnetcore;
using IdentityServer4;

namespace IdentityServer.Oauth
{
    public static class Ids4Extensions
    {
        public static IServiceCollection AddOauths(this IServiceCollection @this)
        {
            @this
                .AddAuthentication()
                .AddDingTalk("dingtalk", "钉钉", SetDingTalkOptions);
            return @this;
        }
        private static void SetDingTalkOptions(DingTalkOAuthOptions options)
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.ClientId = "dingoa9zzmuzo2evbi2jdh";
            options.ClientSecret = "C1xDgV14jc0OVPMFQUnvIzbdM3daHIJqN7MuA5bU9UEmAw1f1pympEzpS6De_tbK";
        }
    }
}
