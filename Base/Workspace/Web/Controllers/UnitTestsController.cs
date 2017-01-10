namespace Web.Controllers
{
    using System;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;

    using Allors;

    public class UnitTestsController : Controller
    {
        private bool IsProduction
        {
            get
            {
                var production = WebConfigurationManager.AppSettings["production"];
                return string.IsNullOrWhiteSpace(production) || bool.Parse(production);
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (!this.Request.IsLocal)
            {
                throw new Exception();
            }

            if (this.IsProduction)
            {
                throw new Exception();
            }

            return base.BeginExecuteCore(callback, state);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}