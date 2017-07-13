namespace Allors.Server.Controllers
{
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class TestAllorsContextController : Controller
    {
        private readonly IAllorsContext allors;

        public TestAllorsContextController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult UserName()
        {
            return this.Ok(this.allors.User.UserName);
        }
    }
}