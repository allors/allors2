namespace Allors.Server.Controllers
{
    using System;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class TestShareHoldersController : Controller
    {
        public TestShareHoldersController(ISessionService sessionService)
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
                var organisation = new Organisations(this.Session).FindBy(M.Organisation.Owner, this.Session.GetUser());
                response.AddObject("root", organisation, M.Organisation.AngularShareholders);
                return this.Ok(response.Build());
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
