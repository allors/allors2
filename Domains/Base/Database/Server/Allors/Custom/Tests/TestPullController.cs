namespace Allors.Server.Controllers
{
    using System;

    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestPullController : Controller
    {
        private readonly IAllorsContext allors;

        public TestPullController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull()
        {
            try
            {
                var response = new PullResponseBuilder(this.allors.User);
                return this.Ok(response.Build());
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}