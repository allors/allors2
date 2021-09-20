// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Typescript
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class Method : IMember
    {
        public Method(JToken json) => this.Json = json;

        public JToken Json { get; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string[] Decorators { get; set; }

        public Parameter[] Parameters { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Type = this.Json["type"]?.Value<string>();

            var jsonDecorators = this.Json["decorators"];
            this.Decorators = jsonDecorators != null
                ? jsonDecorators.Select(v => v.Value<string>()).ToArray()
                : new string[0];

            var jsonParameters = this.Json["parameters"];
            this.Parameters = jsonParameters != null ? jsonParameters.Select(v =>
            {
                var parameter = new Parameter(v);
                parameter.BaseLoad();
                return parameter;
            }).ToArray() : new Parameter[0];
        }
    }
}
