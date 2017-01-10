namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Web.Database;

    public class RelationController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);
            response.AddObject("person", this.AllorsUser);
            return this.JsonSuccess(response.Build());
        }
    }
}