// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Typescript
{
    using Newtonsoft.Json.Linq;

    public class Parameter
    {
        public Parameter(JToken json) => this.Json = json;

        public JToken Json { get; }

        public string Name { get; set; }

        public string Type { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Type = this.Json["type"]?.Value<string>();
        }
    }
}
