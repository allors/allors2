// <copyright file="Module.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using System.Collections.Generic;
    using System.Linq;
    using Autotest.Html;
    using Newtonsoft.Json.Linq;

    public partial class Module
    {
        public Directive[] BootstrapComponents { get; set; }

        public Directive[] DeclaredDirectives { get; set; }

        public Directive[] DeclaredEntryComponents { get; set; }

        public Pipe[] DeclaredPipes { get; set; }

        public Directive[] EntryComponents { get; set; }

        public Directive[] ExportedDirectives { get; set; }

        public Module[] ExportedModules { get; set; }

        public Pipe[] ExportedPipes { get; set; }

        public Directive[] Directives => this.DeclaredDirectives.Union(this.ImportedModules.SelectMany(v => v.ExportedDirectives)).Distinct().ToArray();

        public Module[] ImportedModules { get; set; }

        public JToken Json { get; set; }

        public Project Project { get; set; }

        public Reference Reference { get; set; }

        public Directive[] RoutedComponents { get; set; }

        public Route[] Routes { get; set; }

        public Route[] FlattenedRoutes { get; set; }

        public void BaseLoad()
        {
            var jsonRoutes = this.Json["routes"];
            this.Routes = jsonRoutes != null ? jsonRoutes.Select(v =>
                {
                    var route = new Route(this, v);
                    route.BaseLoad();
                    return route;
                }).ToArray() : new Route[0];

            var pathByRedirectTo = new Dictionary<string, string>();
            foreach (var route in this.Routes)
            {
                route.CreateFullPath("/", pathByRedirectTo);
            }

            var bootstrapComponentIds = Angular.Reference.ParseIds(this.Json["bootstrapComponents"]);
            var entryComponentIds = Angular.Reference.ParseIds(this.Json["entryComponents"]);
            var declaredDirectiveIds = Angular.Reference.ParseIds(this.Json["declaredDirectives"]);
            var exportedDirectiveIds = Angular.Reference.ParseIds(this.Json["exportedDirectives"]);
            var exportedPipeIds = Angular.Reference.ParseIds(this.Json["exportedPipes"]);
            var declaredPipeIds = Angular.Reference.ParseIds(this.Json["declaredPipes"]);
            var importedModuleIds = Angular.Reference.ParseIds(this.Json["importedModules"]);
            var exportedModuleIds = Angular.Reference.ParseIds(this.Json["exportedModules"]);

            this.BootstrapComponents = bootstrapComponentIds.Select(v => this.Project.DirectiveById[v]).ToArray();
            this.EntryComponents = entryComponentIds.Select(v => this.Project.DirectiveById[v]).ToArray();
            this.DeclaredDirectives = declaredDirectiveIds.Select(v => this.Project.DirectiveById[v]).ToArray();
            this.ExportedDirectives = exportedDirectiveIds.Select(v => this.Project.DirectiveById[v]).ToArray();

            this.ExportedPipes = exportedPipeIds.Select(v => this.Project.PipeById[v]).ToArray();
            this.DeclaredPipes = declaredPipeIds.Select(v => this.Project.PipeById[v]).ToArray();

            this.ImportedModules = importedModuleIds.Select(v => this.Project.ModuleById[v]).ToArray();
            this.ExportedModules = exportedModuleIds.Select(v => this.Project.ModuleById[v]).ToArray();

            this.FlattenedRoutes = this.Routes.SelectMany(v => v.Flattened).ToArray();
            this.RoutedComponents = this.FlattenedRoutes.Where(v => v.Component != null).Select(v => v.Component).Distinct().ToArray();
            this.DeclaredEntryComponents = this.EntryComponents.Except(this.RoutedComponents).Except(this.BootstrapComponents).ToArray();
        }

        public Directive LookupComponent(Element element) => this.Directives.FirstOrDefault(v => Equals(element.Name, v.Selector) || Equals(element.Name, v.ExportAs));

        public Directive[] LookupAttributeDirectives(Element element)
        {
            var names = element.Attributes.Select(v => v.Name);
            var component = this.Directives.FirstOrDefault(v => names.Contains(v.ExportAs));

            if (component != null)
            {
                return new[] { component };
            }
            else
            {
                var attributeNames = element.Attributes.Select(v => $"{element}[{v.Name}]");
                return this.Directives.Where(v => attributeNames.Any(w => v.Selector?.Contains(w) ?? false)).ToArray();
            }
        }
    }
}
