// <copyright file="ExpansionCase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class ExpansionCase : INode
    {
        public ExpansionCase(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public INode[] Expression { get; set; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Value = this.Json["value"]?.Value<string>();

            var expressionChildren = this.Json["expression"];
            this.Expression = expressionChildren != null ? expressionChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new INode[0];
        }
    }
}