// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Allors.Workspace.Meta;
    using Autotest.Angular;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public partial class Model
    {
        public MetaPopulation MetaPopulation { get; set; }

        public Dictionary<Guid, MetaExtension> MetaExtensions { get; } = new Dictionary<Guid, MetaExtension>();

        public Project Project { get; set; }

        public Menu Menu { get; set; }

        public ValidationLog Validate() => new ValidationLog();

        public void LoadMetaExtensions(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonMetaExtensions = (JArray)JToken.ReadFrom(reader);

                void Setter(MetaExtension metaExtension, JToken json)
                {
                    metaExtension.List = json["list"]?.Value<string>();
                    metaExtension.Overview = json["overview"]?.Value<string>();
                }

                MetaExtension.Load(this.MetaExtensions, jsonMetaExtensions, Setter);
            }
        }

        public void LoadProject(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonProject = (JObject)JToken.ReadFrom(reader);
                this.Project = new Project
                {
                    Model = this,
                };

                this.Project.Load(jsonProject);
            }
        }

        public void LoadMenu(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonMenu = (JArray)JToken.ReadFrom(reader);

                this.Menu = new Menu
                {
                    Model = this,
                };
                this.Menu.Load(jsonMenu);
            }
        }

        public void LoadDialogs(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonDialogs = JToken.ReadFrom(reader);

                var create = jsonDialogs["create"] as JArray;
                var edit = jsonDialogs["create"] as JArray;

                void CreateSetter(MetaExtension metaExtension, JToken json)
                {
                    metaExtension.Create = json["component"]?.Value<string>();
                }

                MetaExtension.Load(this.MetaExtensions, create, CreateSetter);

                void EditSetter(MetaExtension metaExtension, JToken json)
                {
                    metaExtension.Edit = json["component"]?.Value<string>();
                }

                MetaExtension.Load(this.MetaExtensions, edit, EditSetter);
            }
        }
    }
}
