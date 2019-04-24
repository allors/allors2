namespace Autocomplete
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Allors.Meta;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public partial class Model
    {
        public MetaPopulation MetaPopulation { get; set; }

        public Dictionary<Guid, MetaExtension> MetaExtensions { get; } = new Dictionary<Guid, MetaExtension>();

        public Menu Menu { get; set; }

        public ValidationLog Validate()
        {
            return new ValidationLog();
        }

        public void LoadMeta(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonArray = (JArray)JToken.ReadFrom(reader);

                foreach (var json in jsonArray)
                {
                    if (json["id"] != null)
                    {
                        Guid.TryParse(json["id"].Value<string>(), out var id);
                        if (!this.MetaExtensions.TryGetValue(id, out var metaExtension))
                        {
                            metaExtension = new MetaExtension
                            {
                                Id = id
                            };
                            this.MetaExtensions.Add(id, metaExtension);
                        }

                        metaExtension.List = json["list"]?.Value<string>();
                        metaExtension.Overview = json["overview"]?.Value<string>();
                    }
                }
            }
        }

        public void LoadMenu(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonArray = (JArray)JToken.ReadFrom(reader);

                this.Menu = new Menu
                {
                    Model = this
                };
                this.Menu.Load(jsonArray);
            }
        }
    }
}
