using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Test
{
    [Authorize]
    public class PingController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Token()
        {
            return this.Ok("Pong");
        }
    }
}
