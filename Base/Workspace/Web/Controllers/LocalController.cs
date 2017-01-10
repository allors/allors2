namespace Web.Controllers
{
    using System;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;

    using Allors;

    public class LocalController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (!this.Request.IsLocal)
            {
                throw new Exception();
            }

            return base.BeginExecuteCore(callback, state);
        }

        public bool IsProduction 
        {
            get
            {
                var production = WebConfigurationManager.AppSettings["production"];
                return string.IsNullOrWhiteSpace(production) || bool.Parse(production);
            }
        }

        [HttpGet]
        public ActionResult Login(string user, string returnUrl)
        {
            if (this.IsProduction)
            {
                throw new Exception("Programmatic login is not supported in production");
            }

            if (string.IsNullOrWhiteSpace(user))
            {
                //user = @"guest";
                user = @"administrator";
            }

            FormsAuthentication.SetAuthCookie(user, false);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}