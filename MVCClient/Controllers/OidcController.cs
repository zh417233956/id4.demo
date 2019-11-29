using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCClient.Controllers
{
    [Route("oidc")]
    public class OidcController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("front-channel-logout-callback")]
        public ActionResult FrontChannelLogoutCallback(string sid)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var sessionId = claimsPrincipal?.FindFirst("sid")?.Value;
            if (sessionId != null && sessionId == sid)
            {
                HttpContext.SignOutAsync();
            }

            return Content(this.Request.Host + "退出成功。", "text/html", System.Text.Encoding.UTF8);
        }
    }
}