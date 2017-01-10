namespace Web.Controllers
{
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Web.Database;

    public class OrganisationsController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);

            var organisations = new Organisations(this.AllorsSession).Extent();
            response.AddCollection("organisations", organisations);

            return this.JsonSuccess(response.Build());
        }
    }
}