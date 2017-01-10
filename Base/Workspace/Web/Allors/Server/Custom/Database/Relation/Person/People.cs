namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Web.Database;

    public class PeopleController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var reponse = new PullResponseBuilder(this.AllorsUser);

            var people = new People(this.AllorsSession).Extent();
            reponse.AddCollection("people", people);

            return this.JsonSuccess(reponse.Build());
        }
    }
}