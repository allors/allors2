namespace Web.Controllers
{
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Web.Database;

    public class PeopleSheetController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var responseBuilder = new PullResponseBuilder(this.AllorsUser);

            var people = new People(this.AllorsSession).Extent();
            responseBuilder.AddCollection("people", people);

            return this.JsonSuccess(responseBuilder.Build());
        }
    }
}