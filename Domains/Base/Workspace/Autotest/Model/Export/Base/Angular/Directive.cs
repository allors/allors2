// <copyright file="Directive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using System.Collections.Generic;
    using System.Linq;
    using Autotest.Html;
    using Autotest.Typescript;
    using Newtonsoft.Json.Linq;

    public partial class Directive
    {
        public Dictionary<Element, Directive> ComponentByElement { get; set; }

        public string ExportAs { get; set; }

        public bool IsComponent { get; set; }

        public JToken Json { get; set; }

        public Project Project { get; set; }

        public Reference Reference { get; set; }

        public string Selector { get; set; }

        public Template Template { get; set; }

        public Class Type { get; set; }

        public void BaseLoad()
        {
            this.IsComponent = this.Json["isComponent"]?.Value<bool>() ?? false;
            this.Selector = this.Json["selector"]?.Value<string>();
            this.ExportAs = this.Json["exportAs"]?.Value<string>();

            var jsonTemplate = this.Json["template"];
            this.Template = jsonTemplate != null ? new Template(this, jsonTemplate) : null;
            this.Template?.BaseLoad();

            var typeTemplate = this.Json["type"];
            this.Type = typeTemplate != null ? new Class(this, typeTemplate) : null;
            this.Type?.BaseLoad();

            this.ComponentByElement = this.Template?.Elements
                .Select(v => new
                {
                    element = v,
                    component = this.Project.LookupComponent(this, v),
                })
                .Where(v => v.component != null)
                .ToDictionary(v => v.element, v => v.component);
        }
    }
}