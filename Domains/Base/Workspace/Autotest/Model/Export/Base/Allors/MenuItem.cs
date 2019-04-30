﻿// <copyright file="MenuItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public partial class MenuItem
    {
        public Guid? Id { get; set; }

        public string AssignedTitle { get; set; }

        public string AssignedLink { get; set; }

        public MenuItem[] Children { get; set; }

        public Menu Menu { get; set; }

        public MenuItem Parent { get; set; }

        public Model Model => this.Menu.Model;

        public ObjectType ObjectType => (ObjectType)(this.Id.HasValue ? this.Model.MetaPopulation.Find(this.Id.Value) : null);


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

        public Directive Component
        {
            get
            {
                var link = this.Link;
                var route = this.Menu.Model.Project.FindRouteForFullPath(link);
                return route?.Component;
            }
        }

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
                        Parent = this,
                    };
                    child.Load(v);
                    return child;
                }).ToArray();
        }
    }
}