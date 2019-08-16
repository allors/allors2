// <copyright file="Element.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Html
{
    using System.Collections.Generic;
    using System.Linq;
    using Autotest.Angular;
    using Humanizer;
    using Newtonsoft.Json.Linq;

    public partial class Element : INode
    {
        public Element(JToken json, Template template, INode parent)
        {
            this.Json = json;
            this.Template = template;
            this.Parent = parent;
        }

        public Template Template { get; }

        public Attribute[] Attributes { get; set; }

        public INode[] Children { get; set; }

        public Element[] FlattenedElements => this.Flattened.OfType<Element>().ToArray();

        public Text[] FlattenedText => this.Flattened.OfType<Text>().ToArray();

        public INode[] Flattened => this.Flatten(this.Children).Concat(new[] { this }).ToArray();

        public JToken Json { get; }

        public string Name { get; set; }

        public string PropertyName => this.Name.Dehumanize();

        public INode Parent { get; }

        public Directive Component { get; set; }

        public Directive[] AttributeDirectives { get; set; }

        public Directive[] Directives { get; set; }

        public string InnerText => string.Join(string.Empty, this.FlattenedText.Select(v => v.Value));

        public Element[] Ancestors
        {
            get
            {
                var parentElement = (Element)this.Parent;
                return parentElement != null
                    ? new[] { parentElement }.Concat(parentElement.Ancestors).ToArray()
                    : new Element[0];
            }
        }

        public string Scope => this.Component?.Scope ?? this.Attributes.FirstOrDefault(v => v.Name == "data-test-scope")?.Value;

        public Scope InScope { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();

            var jsonChildren = this.Json["children"];
            this.Children = jsonChildren != null
                ? jsonChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v, this.Template, this);
                    node.BaseLoad();
                    return node;
                }).ToArray()
                : new INode[0];

            var jsonAttributes = this.Json["attributes"];
            this.Attributes = jsonAttributes != null
                ? jsonAttributes.Select(v =>
                {
                    var attribute = new Attribute(v, this.Template, this);
                    attribute.BaseLoad();
                    return attribute;
                }).ToArray()
                : new Attribute[0];
        }

        public void SetInScope(Scope scope)
        {
            this.InScope = scope;
            scope.Nodes.Add(this);

            foreach (var text in this.FlattenedText)
            {
                text.SetInScope(scope);
            }

            if (!string.IsNullOrEmpty(this.Scope))
            {
                var newScope = new Scope { Name = this.Scope };
                foreach (var child in this.Children)
                {
                    child.SetInScope(newScope);
                }
            }
        }

        private IEnumerable<INode> Flatten(IEnumerable<INode> nodes) => nodes.SelectMany(v => v is Element element ? new[] { v }.Concat(this.Flatten(element.Children)) : new[] { v });
    }
}
