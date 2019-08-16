namespace Allors.Server.Controllers
{
    using Allors.Domain;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class PersonController : Controller
    {
        private readonly ISessionService allors;

        public PersonController(ISessionService allorsContext) => this.allors = allorsContext;

        [HttpPost]
        public IActionResult Pull([FromBody] Model model)
        {
            var response = new PullResponseBuilder(this.allors.Session.GetUser());

            var person = this.allors.Session.Instantiate(model.Id);
            response.AddObject("person", person);

            return this.Ok(response.Build());
        }

        public class Model
        {
            public string Id { get; set; }
        }
    }
}
