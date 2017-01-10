namespace Web.Controllers
{
    using System;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;

    using Allors;

    using Workspace.Web;

    public class TestController : Controller
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
        public ActionResult Init()
        {
            if (this.IsProduction)
            {
                throw new Exception("Init is not supported in production");
            }

            Config.TimeShift = null;
            Application.Config();

            return this.View();
        }

        [HttpGet]
        public ActionResult Setup()
        {
            if (this.IsProduction)
            {
                throw new Exception("Init is not supported in production");
            }

            Config.Default.Init();
            using (var session = Config.Default.CreateSession())
            {
                new Setup(session, null).Apply();
                session.Commit();
            }

            return this.View();
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
                user = @"Administrator";
            }

            FormsAuthentication.SetAuthCookie(user, false);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult TimeShift(int days, int hours = 0, int minutes = 0, int seconds = 0)
        {
            if (this.IsProduction)
            {
                throw new Exception("Time shifting is not supported in production");
            }

            Config.TimeShift = new TimeSpan(days, hours, minutes, seconds);

            return this.RedirectToAction("Index", "Home");
        }
    }
}