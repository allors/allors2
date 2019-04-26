// <copyright file="Attribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using Newtonsoft.Json.Linq;

    public class Attribute : INode
    {
        public Attribute(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string Name { get; set; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Value = this.Json["value"]?.Value<string>();
        }
    }
}