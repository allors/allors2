namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestEmployeesController : AllorsController
    {
        public TestEmployeesController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull()
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);
            var organisation = new Organisations(this.AllorsSession).FindBy(M.Organisation.Owner, this.AllorsUser);
            response.AddObject("root", organisation, M.Organisation.AngularEmployees);
            return this.Ok(response.Build());
        }
    }
}