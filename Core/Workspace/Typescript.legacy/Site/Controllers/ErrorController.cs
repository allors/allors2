namespace Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("/error")]
    public class ErrorController : Controller
    {
        public ActionResult Index() =>
            // TODO: Log to the database
            this.View();
    }
}
