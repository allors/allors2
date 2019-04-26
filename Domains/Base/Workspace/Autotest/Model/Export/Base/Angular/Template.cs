// <copyright file="Template.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    using System.Linq;
    using Autotest.Html;
    using Newtonsoft.Json.Linq;

    public class Template
    {
        public Template(Directive directive, JToken json)
        {
            this.Directive = directive;
            this.Json = json;
        }

        public Directive Directive { get; }

        public JToken Json { get; }

        private string Url { get; set; }

        private INode[] Html { get; set; }

        public void BaseLoad()
        {
            this.Url = this.Json["url"]?.Value<string>();

            var jsonHtml = this.Json["html"];
            this.Html = jsonHtml != null ? jsonHtml.Select(v =>
                {
                    var node = NodeFactory.Create(v);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new INode[0];
        }
    }
}