namespace Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            // TODO: Log to the database
            return this.View();
        }
    }
}
