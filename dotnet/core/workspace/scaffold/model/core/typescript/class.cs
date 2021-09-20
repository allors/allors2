// <copyright file="Class.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Typescript
{
    using System.Linq;
    using Autotest.Angular;
    using Newtonsoft.Json.Linq;

    public class Class
    {
        public Class(Directive directive, JToken json)
        {
            this.Directive = directive;
            this.Json = json;
        }

        public Directive Directive { get; set; }

        public JToken Json { get; set; }

        public string Name { get; set; }

        public string[] Bases { get; set; }

        public string[] Decorators { get; set; }

        public IMember[] Members { get; set; }

        internal void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Bases = (this.Json["bases"] as JArray)?.Select(v => v.Value<string>()).ToArray();

            var jsonDecorators = this.Json["decorators"];
            this.Decorators = jsonDecorators != null
                ? jsonDecorators.Select(v => v.Value<string>()).ToArray()
                : new string[0];

            var jsonMembers = this.Json["members"];
            this.Members = jsonMembers != null
                ? jsonMembers.Select(v =>
                {
                    var member = MemberFactory.Create(v);
                    member.BaseLoad();
                    return member;
                }).ToArray()
                : new IMember[0];
        }
    }
}
