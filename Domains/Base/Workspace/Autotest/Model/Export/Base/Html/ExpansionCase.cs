// <copyright file="ExpansionCase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

using Autotest.Angular;

namespace Autotest.Html
{
    using System.Linq;

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
    }
}