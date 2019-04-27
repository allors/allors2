// <copyright file="Text.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using Newtonsoft.Json.Linq;

    public partial class Text : INode
    {
        public Text(JToken json, INode parent)
        {
            this.Json = json;
            this.Parent = parent;
        }

        public JToken Json { get; }

        public INode Parent { get; set; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Value = this.Json["value"]?.Value<string>();
        }
    }
}