using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore.Extensions
{
    public static class StringExtension
    {
        public static string EmptyIfNull(this string @this)
        {
            return @this ?? string.Empty;
        }
    }
}
