namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Web.Database;

    public class TestPullController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            try
            {
                var response = new PullResponseBuilder(this.AllorsUser);
                return this.JsonSuccess(response.Build());
            }
            catch (Exception e)
            {
                return this.JsonError(e.Message);
            }
        }
    }
}