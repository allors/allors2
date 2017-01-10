namespace Workspace.Web
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Allors;

    using global::Web;

    using Web;

    public class Application : System.Web.HttpApplication
    {
        public static void Config()
        {
            var configuration = new Allors.Adapters.Object.SqlClient.Configuration
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["allors"].ConnectionString,
                ObjectFactory = Allors.Config.ObjectFactory,
            };
            Allors.Config.Default = new Allors.Adapters.Object.SqlClient.Database(configuration);
        }

        protected void Application_Start()
        {
            Config();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ViewConfig.Register();

            AllorsConfig.Register();
        }
    }
}
