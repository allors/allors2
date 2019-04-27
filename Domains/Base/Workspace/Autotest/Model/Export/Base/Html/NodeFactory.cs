// <copyright file="NodeFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using System;

    using Newtonsoft.Json.Linq;

    public static class NodeFactory
    {
        public static INode Create(JToken json, INode parent)
        {
            var kind = json["kind"]?.Value<string>();
            switch (kind)
            {
                case "element":
                    return new Element(json, parent);

                case "text":
                    return new Text(json, parent);

                case "comment":
                    return new Comment(json, parent);

                case "expansion":
                    return new Expansion(json, parent);

                default:
                    throw new Exception($"Unknown kind: {kind}");
            }
        }
    }
}