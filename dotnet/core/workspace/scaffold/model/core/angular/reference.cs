// <copyright file="Reference.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

    public partial class Reference
    {
        static readonly Regex EatLeadingDots = new Regex(@"^(\.+)");

        public Reference(JToken json)
        {
            this.Name = json["name"]?.Value<string>();
            this.Path = json["path"]?.Value<string>();

            this.Id = $"{this.Name} | {this.Path}";
        }

        public string Id { get; }

        public bool IsLocal => !this.Path.Contains("node_modules/");

        public string Name { get; }

        public string Namespace
        {
            get
            {
                if (this.Path != null)
                {
                    var path = this.Path.Substring(0, this.Path.LastIndexOf("/")).Replace("/", ".");
                    var eatLeadingDots = EatLeadingDots.Replace(path, "");
                    var escaped = string.Join(".", eatLeadingDots.Split('.').Select(v => v.EscapeReservedKeyword()));
                    return escaped;
                }

                return string.Empty;
            }
        }

        public string Path { get; }

        internal static string ParseId(JToken json) => json != null ? new Reference(json).Id : null;

        internal static string[] ParseIds(JToken json) => json != null ? json.Select(v => new Reference(v).Id).ToArray() : new string[0];
    }
}
