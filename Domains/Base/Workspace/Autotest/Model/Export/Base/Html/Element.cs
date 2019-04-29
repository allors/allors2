// <copyright file="Element.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

using Autotest.Angular;

namespace Autotest.Html
{
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

        public Element[] FlattenedElements => this.Flattened.OfType<Element>().ToArray();

        public Text[] FlattenedText => this.Flattened.OfType<Text>().ToArray();

        public INode[] Flattened => this.Flatten(this.Children).Concat(new[] {this}).ToArray();

        public JToken Json { get; }

        public string Name { get; set; }

        public INode Parent { get; set; }

        public Directive Component { get; set; }

        public Directive[] AttributeDirectives { get; set; }

        public Directive[] Directives { get; set; }

        public string InnerText
        {
            get
            {
                return string.Join(string.Empty, this.FlattenedText.Select(v => v.Value));
            }
        }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();

            var jsonChildren = this.Json["children"];
            this.Children = jsonChildren != null
                ? jsonChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v, this);
                    node.BaseLoad();
                    return node;
                }).ToArray()
                : new INode[0];

            var jsonAttributes = this.Json["attributes"];
            this.Attributes = jsonAttributes != null
                ? jsonAttributes.Select(v =>
                {
                    var attribute = new Attribute(v, this);
                    attribute.BaseLoad();
                    return attribute;
                }).ToArray()
                : new Attribute[0];
        }

        private IEnumerable<INode> Flatten(IEnumerable<INode> nodes)
        {
            return nodes.SelectMany(v => v is Element element ? new[]{v}.Concat(this.Flatten(element.Children)) : new[]{v});
        }
    }
}