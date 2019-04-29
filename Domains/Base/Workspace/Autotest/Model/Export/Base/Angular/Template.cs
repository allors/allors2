// <copyright file="Template.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using System.Linq;
    using Autotest.Html;
    using Newtonsoft.Json.Linq;

    public partial class Template
    {
        public Template(Directive directive, JToken json)
        {
            this.Directive = directive;
            this.Json = json;
        }

        public Directive Directive { get; }

        public Element[] Elements { get; set; }

        public INode[] Html { get; set; }

        public JToken Json { get; }

        public string Url { get; set; }

        public void BaseLoad()
        {
            this.Url = this.Json["url"]?.Value<string>();

            var jsonHtml = this.Json["html"];
            this.Html = jsonHtml != null ? jsonHtml.Select(v =>
                {
                    var node = NodeFactory.Create(v, this, null);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new INode[0];

            this.Elements = this.Html.OfType<Element>().SelectMany(v => v.FlattenedElements).ToArray();
        }
    }
}