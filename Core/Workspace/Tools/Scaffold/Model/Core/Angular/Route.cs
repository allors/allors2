// <copyright file="Route.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class Route
    {
        public Route(Module module, JToken jToken)
        {
            this.Module = module;
            this.Json = jToken;
        }

        public Route[] Children { get; set; }

        public Directive Component { get; set; }

        public JToken Data { get; set; }

        public Route[] Flattened => this.Flatten(this.Children).Concat(new[] { this }).ToArray();

        public JToken Json { get; }

        public Module Module { get; }

        public string Outlet { get; set; }

        public Route Parent { get; set; }

        public string Path { get; set; }

        public string PathMatch { get; set; }

        public string RedirectTo { get; set; }

        public string[] FullPaths { get; set; }

        public void BaseLoad()
        {
            var jsonRoutes = this.Json["children"];
            this.Children = jsonRoutes != null ? jsonRoutes.Select(v =>
                {
                    var route = new Route(this.Module, v)
                    {
                        Parent = this,
                    };
                    route.BaseLoad();
                    return route;
                }).ToArray() : new Route[0];

            this.Path = this.Json["path"]?.Value<string>();
            this.PathMatch = this.Json["pathMatch"]?.Value<string>();
            this.RedirectTo = this.Json["redirectTo"]?.Value<string>();
            this.Outlet = this.Json["outlet"]?.Value<string>();
            this.Data = this.Json["data"]?.Value<string>();

            var componentId = Angular.Reference.ParseId(this.Json["component"]);
            this.Component = componentId != null ? this.Module.Project.DirectiveById[componentId] : null;
        }

        public void CreateFullPath(string parentPath, Dictionary<string, string> pathByRedirectTo)
        {
            var path = parentPath + this.Path;

            if (this.RedirectTo != null)
            {
                pathByRedirectTo[this.RedirectTo] = path;
                this.FullPaths = new string[0];
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Path))
                {
                    this.FullPaths = pathByRedirectTo.TryGetValue(path, out var redirectTo) ?
                        new[] { redirectTo, path } :
                        new[] { path };
                }
                else
                {
                    this.FullPaths = new string[0];
                }
            }

            var childParentPath = path.EndsWith("/") ? path : path + "/";
            foreach (var child in this.Children)
            {
                child.CreateFullPath(childParentPath, pathByRedirectTo);
            }
        }

        private IEnumerable<Route> Flatten(IEnumerable<Route> routes) => routes.SelectMany(v => this.Flatten(v.Children)).Concat(routes);
    }
}
