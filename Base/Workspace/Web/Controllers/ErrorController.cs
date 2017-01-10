namespace Web.Error
{
    using System.Web.Mvc;

    public class ErrorController : CustomController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}