namespace Allors.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        public ISession AllorsSession { get; set; }

        public TestController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
        }

        [HttpGet]
        public IActionResult Init()
        {
            this.AllorsSession.Database.Init();

            return this.Ok("Init");
        }

        [HttpGet]
        public IActionResult Setup()
        {
            this.AllorsSession.Database.Init();
            using (var session = this.AllorsSession.Database.CreateSession())
            {
                new Setup(session, null).Apply();
                session.Commit();
            }

            return this.Ok("Setup");
        }

        [HttpGet]
        public IActionResult Login(string user, string returnUrl)
        {
            // TODO:
            return this.Ok("Login");
        }

        [HttpGet]
        public IActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            // TODO:
            return this.Ok("TimeShift");
        }
    }
}
