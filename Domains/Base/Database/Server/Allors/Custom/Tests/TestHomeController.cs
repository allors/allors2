namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestHomeController : AllorsController
    {
        public TestHomeController(IAllorsContext allorsContext) : base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull()
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);
            var organisation = new Organisations(this.AllorsSession).FindBy(M.Organisation.Owner, this.AllorsUser);
            response.AddObject("root", organisation, M.Organisation.AngularShareholders);
            return this.Ok(response.Build());
        }
    }
}