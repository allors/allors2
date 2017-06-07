namespace Allors.Server.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestPullController : AllorsController
    {
        public TestPullController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull()
        {
            await this.OnInit();

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