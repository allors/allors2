namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestNoTreeController : Controller
    {
        private IAllorsContext allors;

        public TestNoTreeController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull()
        {
            var response = new PullResponseBuilder(this.allors.User);
            response.AddObject("object", this.allors.User);
            response.AddCollection("collection", new Organisations(this.allors.Session).Extent());
            return this.Ok(response.Build());
        }
    }
}