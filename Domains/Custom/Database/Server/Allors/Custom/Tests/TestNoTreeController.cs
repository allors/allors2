using Microsoft.AspNetCore.Mvc;

namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Server;

    public class TestNoTreeController : PullController
    {
        public TestNoTreeController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);
            response.AddObject("object", this.AllorsUser);
            response.AddCollection("collection", new Organisations(this.AllorsSession).Extent());
            return this.Ok(response.Build());
        }
    }
}