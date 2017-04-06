using Microsoft.AspNetCore.Mvc;

namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    public class TestHomeController : PullController
    {
        public TestHomeController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);
            var organisation = new Organisations(this.AllorsSession).FindBy(M.Organisation.Owner, this.AllorsUser);
            response.AddObject("root", organisation, M.Organisation.AngularShareholders);
            return this.Ok(response.Build());
        }
    }
}