namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Web.Database;

    public class TestNoTreeController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);
            response.AddObject("object", this.AllorsUser);
            response.AddCollection("collection", new Organisations(this.AllorsSession).Extent());
            return this.JsonSuccess(response.Build());
        }
    }
}