using Newtonsoft.Json.Linq;

namespace Autotest.Angular
{
    public partial class Project
    {
        public void Load(JObject jsonProject)
        {
            this.BaseLoad(jsonProject);
            this.CustomLoad(jsonProject);
        }
    }
}