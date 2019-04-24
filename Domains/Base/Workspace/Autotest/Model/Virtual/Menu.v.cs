namespace Autocomplete
{
    using Newtonsoft.Json.Linq;

    public partial class Menu
    {
        public void Load(JArray json)
        {
            this.BaseLoad(json);
            this.CustomLoad(json);
        }
    }
}
