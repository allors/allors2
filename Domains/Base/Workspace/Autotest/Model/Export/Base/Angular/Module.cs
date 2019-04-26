// <copyright file="Module.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class Module
    {
        public Project Project { get; set; }

        public JToken Json { get; set; }

        public Reference Reference { get; set; }

        public Directive[] BootstrapComponents { get; set; }

        public Directive[] DeclaredDirectives { get; set; }

        public Directive[] ExportedDirectives { get; set; }

        public Pipe[] ExportedPipes { get; set; }

        public Pipe[] DeclaredPipes { get; set; }

        public Module[] ImportedModules { get; set; }

        public Module[] ExportedModules { get; set; }

        public Directive[] EntryComponents { get; set; }

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
        }
    }
}