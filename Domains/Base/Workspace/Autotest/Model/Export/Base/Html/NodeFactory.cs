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
        public static INode Create(JToken json)
        {
            var kind = json["kind"]?.Value<string>();
            switch (kind)
            {
                case "element":
                    return new Element(json);
                case "text":
                    return new Text(json);
                case "comment":
                    return new Comment(json);
                case "expansion":
                    return new Expansion(json);
                default:
                    throw new Exception($"Unknown kind: {kind}");
            }
        }
    }
}