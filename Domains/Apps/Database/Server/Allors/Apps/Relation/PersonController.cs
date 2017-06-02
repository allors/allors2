namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class PersonController : PullController
    {
        public PersonController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull([FromBody] Model model)
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            var person = this.AllorsSession.Instantiate(model.Id);
            response.AddObject("person", person);

            return this.Ok(response.Build());
        }

        public class Model
        {
            public string Id { get; set; }
        }
    }
}