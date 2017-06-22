namespace Allors.Server.Controllers
{
    using System;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestShareHoldersController : Controller
    {
        private readonly IAllorsContext allors;

        public TestShareHoldersController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull()
        {
            try
            {
                var response = new PullResponseBuilder(this.allors.User);
                var organisation = new Organisations(this.allors.Session).FindBy(M.Organisation.Owner, this.allors.User);
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