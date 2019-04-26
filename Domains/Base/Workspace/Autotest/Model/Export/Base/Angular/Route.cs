namespace Autotest.Angular
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Route
    {
        public Module Module { get; }

        public JToken Json { get; }

        public Route Parent { get; set; }

        public string Path { get; set; }

        public string PathMatch { get; set; }

        public Directive Component { get; set; }

        public string RedirectTo { get; set; }

        public string Outlet { get; set; }

        public JToken Data { get; set; }

        public Route[] Children { get; set; }

        public Route(Module module, JToken jToken)
        {
            this.Module = module;
            this.Json = jToken;
        }

        public void BaseLoad()
        {
            var jsonRoutes = this.Json["routes"];
            this.Children = jsonRoutes != null ? jsonRoutes.Select(v =>
                {
                    var route = new Route(this.Module, v)
                                    {
                                        Parent = this
                                    };
                    route.BaseLoad();
                    return route;
                }).ToArray() : new Route[0];
        }
    }
}