namespace Autotest
{
    using Allors.Meta;

    using Newtonsoft.Json.Linq;

    public partial class MenuItem
    {
        public Menu Menu { get; set; }

        public MenuItem Parent { get; set; }

        public Model Model => this.Menu.Model;

        public ObjectType ObjectType => (ObjectType)(this.Id.HasValue ? this.Model.MetaPopulation.Find(this.Id.Value) : null);

        public void Load(JObject json)
        {
            this.BaseLoadMenu(json);
            this.CustomLoadMenu(json);
        }
    }
}
