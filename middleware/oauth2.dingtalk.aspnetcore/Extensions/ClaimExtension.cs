using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace oauth2.dingtalk.aspnetcore.Extensions
{
    public static class ClaimExtension
    {
        public static string FindFirstValue(this ICollection<Claim> @this, string type)
        {
            return @this?.FirstOrDefault(claim => claim.Type == type)?.Value;
        }
    }
}
