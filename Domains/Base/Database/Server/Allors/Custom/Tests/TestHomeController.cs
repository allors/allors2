namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestHomeController : Controller
    {
        private readonly IAllorsContext allors;

        public TestHomeController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull()
        {
            var response = new PullResponseBuilder(this.allors.User);
            var organisation = new Organisations(this.allors.Session).FindBy(M.Organisation.Owner, this.allors.User);
            response.AddObject("root", organisation, M.Organisation.AngularShareholders);
            return this.Ok(response.Build());
        }
    }
}