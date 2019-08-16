// <copyright file="ExpansionCase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Html
{
    using System.Linq;
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public partial class ExpansionCase : INode
    {
        public ExpansionCase(JToken json, Template template, INode parent)
        {
            this.Json = json;
            this.Template = template;
            this.Parent = parent;
        }

        public INode[] Expression { get; set; }

        public JToken Json { get; }

        public Template Template { get; }

        public INode Parent { get; set; }

        public string Value { get; set; }

        public Scope InScope { get; set; }

        public void BaseLoad()
        {
            this.Value = this.Json["value"]?.Value<string>();

            var expressionChildren = this.Json["expression"];
            this.Expression = expressionChildren != null ? expressionChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v, this.Template, this);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new INode[0];
        }

        public void SetInScope(Scope scope)
        {
            this.InScope = scope;
            scope.Nodes.Add(this);
        }
    }
}
