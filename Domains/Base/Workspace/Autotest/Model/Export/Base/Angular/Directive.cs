// <copyright file="Directive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using Autotest.Typescript;
    using Newtonsoft.Json.Linq;

    public class Directive
    {
        public Project Project { get; set; }

        public JToken Json { get; set; }

        public Reference Reference { get; set; }

        public bool IsComponent { get; set; }

        public string Selector { get; set; }

        public Template Template { get; set; }

        public Class Type { get; set; }

        public void BaseLoad()
        {
            this.IsComponent = this.Json["isComponent"]?.Value<bool>() ?? false;
            this.Selector = this.Json["isComponent"]?.Value<string>();

            var jsonTemplate = this.Json["template"];
            this.Template = jsonTemplate != null ? new Template(this, jsonTemplate) : null;
            this.Template?.BaseLoad();

            var typeTemplate = this.Json["type"];
            this.Type = typeTemplate != null ? new Class(this, typeTemplate) : null;
            this.Type?.BaseLoad();
        }
    }
}