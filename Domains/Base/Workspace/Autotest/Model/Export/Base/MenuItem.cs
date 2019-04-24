namespace Autocomplete
{
    using System;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class MenuItem
    {
        public Guid? Id { get; set; }

        public string AssignedTitle { get; set; }

        public string AssignedLink { get; set; }

        public MenuItem[] Children { get; set; }

        public MetaExtension MetaExtension
        {
            get
            {
                if (this.ObjectType != null)
                {
                    this.Model.MetaExtensions.TryGetValue(this.ObjectType.Id, out var metaExtension);
                    return metaExtension;
                }

                return null;
            }
        }

        public string Title => this.AssignedTitle ?? this.ObjectType?.PluralName;

        public string Link => this.AssignedLink ?? this.MetaExtension?.List;

        public void BaseLoadMenu(JObject json)
        {
            if (json["id"] != null)
            {
                Guid.TryParse(json["id"].Value<string>(), out var id);
                this.Id = id;
            }

            this.AssignedTitle = json["title"]?.Value<string>();
            this.AssignedLink = json["link"]?.Value<string>();

            this.Children = json["children"]?.Cast<JObject>()
                .Select(v =>
                {
                    var child = new MenuItem
                    {
                        Menu = this.Menu,
                        Parent = this
                    };
                    child.Load(v);
                    return child;
                }).ToArray();
        }
    }
}
