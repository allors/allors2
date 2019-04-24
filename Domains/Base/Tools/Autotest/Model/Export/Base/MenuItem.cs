namespace Autocomplete
{
    using System;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class MenuItem
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public MenuItem[] Children { get; set; }

        public void BaseLoadMenu(JObject json)
        {
            if (json["id"] != null)
            {
                Guid.TryParse(json["id"].Value<string>(), out var id);
                this.Id = id;
            }

            this.Title = json["title"]?.Value<string>();
            this.Link = json["link"]?.Value<string>();

            this.Children = json["children"]?.Cast<JObject>()
                .Select(v =>
                {
                    var child = new MenuItem();
                    child.Load(v);
                    return child;
                }).ToArray();
        }
    }
}
