// <copyright file="Property.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Typescript
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class Property : IMember
    {
        public Property(JToken json) => this.Json = json;

        public JToken Json { get; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string[] Decorators { get; set; }

        public string Initializer { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Type = this.Json["type"]?.Value<string>();
            this.Initializer = this.Json["initializer"]?.Value<string>();

            var jsonDecorators = this.Json["decorators"];
            this.Decorators = jsonDecorators != null
                ? jsonDecorators.Select(v => v.Value<string>()).ToArray()
                : new string[0];
        }
    }
}
