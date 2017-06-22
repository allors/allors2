namespace Allors.Server.Controllers
{
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class PersonController : Controller
    {
        private IAllorsContext allors;

        public PersonController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull([FromBody] Model model)
        {
            var response = new PullResponseBuilder(this.allors.User);

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