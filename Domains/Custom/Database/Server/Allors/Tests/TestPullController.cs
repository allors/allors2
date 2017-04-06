using Microsoft.AspNetCore.Mvc;

namespace Allors.Server.Controllers
{
    using System;
    using Allors.Server;

    public class TestPullController : PullController
    {
        public TestPullController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public ActionResult Pull()
        {
            try
            {
                var response = new PullResponseBuilder(this.AllorsUser);
                return this.Ok(response.Build());
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}