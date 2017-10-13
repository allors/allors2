namespace Allors.Server.Controllers
{
    using System;

    using Allors.Domain;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class TestPullController : Controller
    {
        public TestPullController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
        }

        private ISession Session { get; }

        [HttpPost]
        public IActionResult Pull()
        {
            try
            {
                var response = new PullResponseBuilder(this.Session.GetUser());
                return this.Ok(response.Build());
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}