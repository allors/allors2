namespace Autocomplete
{
    using System.IO;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public partial class MenuItem
    {
        public void Load(JObject json)
        {
            this.BaseLoadMenu(json);
            this.CustomLoadMenu(json);
        }
    }
}
