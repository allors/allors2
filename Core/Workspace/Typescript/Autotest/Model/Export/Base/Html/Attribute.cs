// <copyright file="Attribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Html
{
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public partial class Attribute : INode
    {
        public Attribute(JToken json, Template template, INode parent)
        {
            this.Json = json;
            this.Template = template;
            this.Parent = parent;
        }

        public JToken Json { get; }

        public Template Template { get; }

        public string Name { get; set; }

        public INode Parent { get; set; }

        public string Value { get; set; }

        public Scope InScope { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Value = this.Json["value"]?.Value<string>();
        }

        public void SetInScope(Scope scope)
        {
            this.InScope = scope;
            scope.Nodes.Add(this);
        }
    }
}
