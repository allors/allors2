namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Server;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PeopleController : Controller
    {
        private readonly IAllorsContext allors;

        public PeopleController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Pull()
        {
            var response = new PullResponseBuilder(this.allors.User);
            var people = new People(this.allors.Session).Extent().ToArray();
            response.AddCollection("people", people);
            return this.Ok(response.Build());
        }
    }
}