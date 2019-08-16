namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class TestNoTreeController : Controller
    {
        public TestNoTreeController(ISessionService sessionService)
        {
            this.Session = sessionService.Session;
        }

        private ISession Session { get; }

        [HttpPost]
        public IActionResult Pull()
        {
            var user = this.Session.GetUser();
            var response = new PullResponseBuilder(user);
            response.AddObject("object", user);
            response.AddCollection("collection", new Organisations(this.Session).Extent());
            return this.Ok(response.Build());
        }
    }
}
