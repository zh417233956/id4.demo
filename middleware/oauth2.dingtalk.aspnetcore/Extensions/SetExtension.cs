using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore.Extensions
{
    public static class SetExtension
    {
        public static ISet<T> AddAll<T>(this ISet<T> @this, ISet<T> other)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (other == null)
            {
                return @this;
            }

            foreach (var item in other)
            {
                @this.Add(item);
            }

            return @this;
        }
    }
}
