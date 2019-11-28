using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Auths
{
    public class CustomsAuthorizationRequirement:IAuthorizationRequirement
    {
        public CustomsAuthorizationRequirement()
        { 
        }
    }
}
