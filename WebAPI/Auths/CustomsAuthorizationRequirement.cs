using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Auths
{
    public class CustomsAuthorizationRequirement:IAuthorizationRequirement
    {
        public CustomsAuthorizationRequirement()
        { 
        }
    }
}
