using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Auths
{
    public class CustomsAuthorizationHandler : AuthorizationHandler<CustomsAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomsAuthorizationRequirement requirement)
        {
            //var filterContext = context.Resource as AuthorizationHandlerContext;
            //if (filterContext == null)
            //{
            //    context.Fail();
            //    return Task.CompletedTask;
            //}
            //context.Succeed(requirement);
            //return Task.CompletedTask;

            var role = context.User.Claims.FirstOrDefault(m => m.Type == "role")?.Value;
            var idp = context.User.Claims.FirstOrDefault(m => m.Type == "idp")?.Value;
            if (context.User.Identity.IsAuthenticated && (idp == "dingtalk" || role == "admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
