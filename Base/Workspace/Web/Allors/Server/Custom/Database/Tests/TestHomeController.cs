namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Web.Database;

    public class TestHomeController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var response = new PullResponseBuilder(this.AllorsUser);
            var organisation = new Organisations(this.AllorsSession).FindBy(M.Organisation.Owner, this.AllorsUser);
            response.AddObject("root", organisation, M.Organisation.AngularShareholders);
            return this.JsonSuccess(response.Build());
        }
    }
}