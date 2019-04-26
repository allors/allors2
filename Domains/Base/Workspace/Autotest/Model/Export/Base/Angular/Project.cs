using System.Linq;
using Allors.Meta;
using Newtonsoft.Json.Linq;

namespace Autotest.Angular
{
    public partial class Project
    {
        public Model Model { get; set; }

        public Module[] Modules { get; set; }
        public Pipe[] Pipes{ get; set; }
        public Provider[] Providers{ get; set; }
        public Directive[] Directives{ get; set; }

        private void BaseLoad(JObject jsonProject)
        {
            var jsonModules = jsonProject["modules"];
            this.Modules = jsonModules != null ? jsonModules.Select(v => 
            { 
                var module = new Module { Project = this};
                return module;
            }).ToArray() : new Module[0];
        }
    }
}