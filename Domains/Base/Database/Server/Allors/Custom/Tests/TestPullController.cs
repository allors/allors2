using Microsoft.AspNetCore.Mvc;

namespace Allors.Server.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Allors.Server;

    public class TestPullController : PullController
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