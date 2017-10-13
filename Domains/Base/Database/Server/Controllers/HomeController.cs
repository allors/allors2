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
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
