// <copyright file="Comment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    using Newtonsoft.Json.Linq;

    public class Comment : INode
    {
        public Comment(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Value = this.Json["value"]?.Value<string>();
        }
    }
}