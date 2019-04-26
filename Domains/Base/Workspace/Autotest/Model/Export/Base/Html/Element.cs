// <copyright file="Element.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Element : INode
    {
        public Element(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string Name { get; set; }

        public INode[] Children { get; set; }

        public Attribute[] Attributes { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();

            var jsonChildren = this.Json["children"];
            this.Children = jsonChildren != null ? jsonChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new INode[0];

            var jsonAttributes = this.Json["attributes"];
            this.Attributes = jsonAttributes != null ? jsonAttributes.Select(v =>
                {
                    var attribute = new Attribute(v);
                    attribute.BaseLoad();
                    return attribute;
                }).ToArray() : new Attribute[0];
        }
    }
}