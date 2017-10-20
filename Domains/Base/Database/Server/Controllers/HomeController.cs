namespace Identity.Controllers
{
    using System.Diagnostics;

    using Allors.Services;

    using Identity.Models;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public HomeController(ISessionService sessionService)
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return this.View();
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
