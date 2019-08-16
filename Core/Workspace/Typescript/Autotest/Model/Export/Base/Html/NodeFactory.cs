// <copyright file="NodeFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System;
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public static class NodeFactory
    {
        public static INode Create(JToken json, Template template, INode parent)
        {
            var kind = json["kind"]?.Value<string>();
            switch (kind)
            {
                case "element":
                    return new Element(json, template, parent);

                case "text":
                    return new Text(json, template, parent);

                case "comment":
                    return new Comment(json, template, parent);

                case "expansion":
                    return new Expansion(json, template, parent);

                default:
                    throw new Exception($"Unknown kind: {kind}");
            }
        }
    }
}
