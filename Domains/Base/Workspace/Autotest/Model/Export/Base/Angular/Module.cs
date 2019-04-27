// <copyright file="Module.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using System;
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

        public Module[] ImportedModules { get; set; }

        public JToken Json { get; set; }

        public Project Project { get; set; }

        public Reference Reference { get; set; }

        public Directive[] RoutedComponents { get; set; }

        public Route[] Routes { get; set; }

        public void BaseLoad()
        {
            var jsonRoutes = this.Json["routes"];
            this.Routes = jsonRoutes != null ? jsonRoutes.Select(v =>
                {
                    var route = new Route(this, v);
                    route.BaseLoad();
                    return route;
                }).ToArray() : new Route[0];

            var bootstrapComponentIds = Angular.Reference.ParseIds(this.Json["bootstrapComponents"]);
            var declaredDirectiveIds = Angular.Reference.ParseIds(this.Json["declaredDirectives"]);
            var exportedDirectiveIds = Angular.Reference.ParseIds(this.Json["exportedDirectives"]);
            var exportedPipeIds = Angular.Reference.ParseIds(this.Json["exportedPipes"]);
            var declaredPipeIds = Angular.Reference.ParseIds(this.Json["declaredPipes"]);
            var importedModuleIds = Angular.Reference.ParseIds(this.Json["importedModules"]);
            var exportedModuleIds = Angular.Reference.ParseIds(this.Json["exportedModules"]);
            var entryComponentIds = Angular.Reference.ParseIds(this.Json["entryComponents"]);

            this.BootstrapComponents = bootstrapComponentIds.Select(v => this.Project.DirectiveById[v]).ToArray();
            this.DeclaredDirectives = declaredDirectiveIds.Select(v => this.Project.DirectiveById[v]).ToArray();
            this.ExportedDirectives = exportedDirectiveIds.Select(v => this.Project.DirectiveById[v]).ToArray();

            this.ExportedPipes = exportedPipeIds.Select(v => this.Project.PipeById[v]).ToArray();
            this.DeclaredPipes = declaredPipeIds.Select(v => this.Project.PipeById[v]).ToArray();

            this.ImportedModules = importedModuleIds.Select(v => this.Project.ModuleById[v]).ToArray();
            this.ExportedModules = exportedModuleIds.Select(v => this.Project.ModuleById[v]).ToArray();

            this.EntryComponents = entryComponentIds.Select(v => this.Project.DirectiveById[v]).ToArray();

            var flattenedRoutes = this.Routes.SelectMany(v => v.Flattened).ToArray();
            this.RoutedComponents = flattenedRoutes.Where(v => v.Component != null).Select(v => v.Component).Distinct().ToArray();
            this.DeclaredEntryComponents = this.EntryComponents.Except(this.RoutedComponents).Except(this.BootstrapComponents).ToArray();
        }

        public Directive LookupComponent(Element element)
        {
            var declaredDirectives = this.DeclaredDirectives.Union(this.ImportedModules.SelectMany(v => v.DeclaredDirectives));

            var names = new[] { element.Name }.Concat(element.Attributes.Select(v => v.Name));
            var component = declaredDirectives.FirstOrDefault(v => names.Contains(v.Selector) || names.Contains(v.ExportAs));

            if (component == null)
            {
                var attributeNames = element.Attributes.Select(v => $"{element}[{v.Name}]");
                component = declaredDirectives.FirstOrDefault(v => attributeNames.Any(w => v.Selector?.Contains(w) ?? false));
            }

            return component;
        }
    }
}