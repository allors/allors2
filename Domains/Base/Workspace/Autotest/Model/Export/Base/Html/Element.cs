// <copyright file="Element.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public partial class Element : INode
    {
        public Element(JToken json, INode parent)
        {
            this.Json = json;
            this.Parent = parent;
        }

        public Attribute[] Attributes { get; set; }

        public INode[] Children { get; set; }

        public Element[] ElementChildren => this.Children.OfType<Element>().ToArray();

        public Element[] Flattened
        {
            get
            {
                return this.Flatten(this.ElementChildren).Concat(new[] { this }).ToArray();
            }
        }

        public JToken Json { get; }

        public string Name { get; set; }

        public INode Parent { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();

            var jsonChildren = this.Json["children"];
            this.Children = jsonChildren != null ? jsonChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v, this);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new INode[0];

            var jsonAttributes = this.Json["attributes"];
            this.Attributes = jsonAttributes != null ? jsonAttributes.Select(v =>
                {
                    var attribute = new Attribute(v, this);
                    attribute.BaseLoad();
                    return attribute;
                }).ToArray() : new Attribute[0];
        }

        private IEnumerable<Element> Flatten(IEnumerable<Element> elements)
        {
            return elements.SelectMany(v => this.Flatten(v.ElementChildren)).Concat(elements);
        }
    }
}