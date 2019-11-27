using oauth2.dingtalk.aspnetcore.Response;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using oauth2.dingtalk.aspnetcore.Extensions;

namespace oauth2.dingtalk.aspnetcore
{
    public sealed partial class DingTalkConnectProfile
    {
        public static DingTalkConnectProfile From(ICollection<Claim> claims)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            return new DingTalkConnectProfile
            {
                Claims = claims,
                Issuer = claims.FindFirstValue(ClaimTypes.Issuer),
                ClientId = claims.FindFirstValue(ClaimTypes.ClientId),
                OpenId = claims.FindFirstValue(ClaimTypes.OpenId),
                UnionId = claims.FindFirstValue(ClaimTypes.UnionId),
                DingId = claims.FindFirstValue(ClaimTypes.DingId),
                NickName = claims.FindFirstValue(ClaimTypes.NickName),
                ExpiresIn = int.Parse(claims.FindFirstValue(ClaimTypes.ExpiresIn))
            };
        }

        public static DingTalkConnectProfile From(string issuer, string clientId, DingTalk_User_ResultEntity openId)
        {
            if (issuer == null)
            {
                throw new ArgumentNullException(nameof(issuer));
            }

            if (openId == null)
            {
                throw new ArgumentNullException(nameof(openId));
            }

            return new DingTalkConnectProfile
            {
                Issuer = issuer,
                ClientId = clientId,
                UnionId = openId.unionid,
                OpenId = openId.openid,
                DingId = openId.dingid,
                NickName = openId.nick,
                ExpiresIn = 60 * 60 * 2
            };
        }

        public string Issuer { get; private set; }

        public string ClientId { get; private set; }

        public string DingId { get; private set; }

        public string OpenId { get; private set; }

        public string UnionId { get; private set; }

        public string NickName { get; private set; }

        public int ExpiresIn { get; private set; }

        private ICollection<Claim> _claims;

        public ICollection<Claim> Claims
        {
            get
            {
                if (_claims == null)
                {
                    _claims = new List<Claim>
                    {
                        BuildClaim(ClaimTypes.Issuer, Issuer),
                        BuildClaim(System.Security.Claims.ClaimTypes.Sid,UnionId),
                        BuildClaim(System.Security.Claims.ClaimTypes.NameIdentifier, UnionId),
                        BuildClaim(System.Security.Claims.ClaimTypes.Name,NickName),
                        BuildClaim(ClaimTypes.ClientId, ClientId),
                        BuildClaim(ClaimTypes.OpenId, OpenId),
                        BuildClaim(ClaimTypes.UnionId, UnionId),
                        BuildClaim(ClaimTypes.DingId, DingId),
                        BuildClaim(ClaimTypes.NickName, NickName),
                        BuildClaim(ClaimTypes.ExpiresIn, ExpiresIn.ToString())
                    };
                }
                return _claims;
            }
            private set => _claims = value;
        }

        public ClaimsIdentity BuildClaimsIdentity()
        {
            return new ClaimsIdentity(
                Claims,
                Issuer,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        }

        public ClaimsPrincipal BuildClaimsPrincipal()
        {
            return new ClaimsPrincipal(BuildClaimsIdentity());
        }

        private Claim BuildClaim(string type, string value)
        {
            return new Claim(type, value.EmptyIfNull(), ClaimValueTypes.String, Issuer);
        }
    }
}
