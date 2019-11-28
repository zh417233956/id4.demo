using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore
{
    public sealed partial class DingTalkConnectProfile
    {
        public static class ClaimTypes
        {
            public static readonly string Prefix = "";

            public static readonly string Issuer = Prefix + "issuer";

            public static readonly string ClientId = Prefix + "client_id";

            public static readonly string OpenId = Prefix + "openid";

            public static readonly string UnionId = Prefix + "unionid";

            public static readonly string DingId = Prefix + "dingid";

            public static readonly string ExpiresIn = Prefix + "expires_in";

            public static readonly string NickName = Prefix + "nickname";
        }
    }
}
