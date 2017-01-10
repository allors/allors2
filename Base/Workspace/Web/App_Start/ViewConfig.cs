namespace Workspace.Web
{
    using System.Web.Mvc;

    public class ViewConfig
    {
        public static void Register()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new AllorsRazorViewEngine());
        }
        
        private class AllorsRazorViewEngine : RazorViewEngine
        {
            public AllorsRazorViewEngine()
            {
                this.ViewLocationFormats = new[]
{
"~/Views/{1}/{0}.cshtml",
"~/Allors/Server/Custom/{1}/{0}.cshtml",
"~/Allors/Server/Base/{1}/{0}.cshtml",
"~/Views/Shared/{0}.cshtml",
"~/Allors/Server/Custom/Shared/{0}.cshtml",
"~/Allors/Server/Base/Shared/{0}.cshtml",
};

                this.MasterLocationFormats = this.ViewLocationFormats;

                this.PartialViewLocationFormats = new[]
{
"~/Views/{1}/{0}.cshtml",
"~/Allors/Server/Custom/{1}/{0}.cshtml",
"~/Allors/Server/Base/{1}/{0}.cshtml",
"~/Views/Shared/{0}.cshtml",
"~/Allors/Server/Custom/Shared/{0}.cshtml",
"~/Allors/Server/Base/Shared/{0}.cshtml",
};
            }
        }
    }
}
