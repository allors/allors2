namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class PeopleController : AllorsController
    {
        public PeopleController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull()
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            var people = new People(this.AllorsSession).Extent();
            response.AddCollection("people", people);

            return this.Ok(response.Build());
        }
    }
}