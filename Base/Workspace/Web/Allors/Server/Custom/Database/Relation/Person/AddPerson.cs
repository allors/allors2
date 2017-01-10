namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Web.Database;

    public class AddPersonController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);
            return this.JsonSuccess(response.Build());
        }
    }
}