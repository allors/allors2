// <copyright file="Parameter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
