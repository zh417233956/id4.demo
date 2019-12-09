using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Auths
{
    public class CustomsAuthorizationHandler : AuthorizationHandler<CustomsAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomsAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var resource_access = context.User.Claims.FirstOrDefault(m => m.Type == "resource_access")?.Value;
            JObject resource_access_json = JObject.Parse(resource_access);

            var roles_json = resource_access_json.First.FirstOrDefault()?.FirstOrDefault()?.FirstOrDefault();
            List<string> roles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(roles_json.ToString());

            if (roles.Contains("admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
