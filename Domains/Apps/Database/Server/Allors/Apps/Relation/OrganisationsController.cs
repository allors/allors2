
namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class OrganisationsController : AllorsController
    {
        public OrganisationsController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull()
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            var organisations = new Organisations(this.AllorsSession).Extent();
            response.AddCollection("organisations", organisations, M.Organisation.AngularList);

            return this.Ok(response.Build());
        }
    }
}