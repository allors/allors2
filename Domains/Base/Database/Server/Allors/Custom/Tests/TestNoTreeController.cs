namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestNoTreeController : AllorsController
    {
        public TestNoTreeController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull()
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);
            response.AddObject("object", this.AllorsUser);
            response.AddCollection("collection", new Organisations(this.AllorsSession).Extent());
            return this.Ok(response.Build());
        }
    }
}