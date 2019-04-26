namespace Autotest.Angular
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Module
    {
        public Project Project { get; set; }

        public JToken Json { get; set; }

        public Reference Reference { get; set; }

        public Directive[] BootstrapComponents { get; set; }

        public Directive[] DeclaredDirectives { get; set; }

        public Directive[] ExportedDirectives { get; set; }

        public Pipe[] ExportedPipes { get; set; }

        public Pipe[] DeclaredPipes { get; set; }
        
        public Module[] ImportedModules { get; set; }

        public Module[] ExportedModules { get; set; }

        public Route[] Routes { get; set; }

        public void BaseLoad()
        {
            var jsonRoutes = this.Json["routes"];
            this.Routes = jsonRoutes != null ? jsonRoutes.Select(v =>
                {
                    var route = new Route(this, v);
                    route.BaseLoad();
                    return route;
                }).ToArray() : new Route[0];
        }
    }
}