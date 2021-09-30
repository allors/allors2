// <copyright file="AllorsMaterialTableTester.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;
    using Autotest.Typescript;

    public partial class AllorsMaterialTableTester : Tester
    {
        public AllorsMaterialTableTester(Element element)
            : base(element)
        {
            var binding = this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == "[table]")?.Value;
            var type = this.Element.Template.Directive.Type;
            var property = type.Members.OfType<Property>().FirstOrDefault(v => v.Name == binding);
        }

        public override string PropertyName => "Table";

        public string Selector => !string.IsNullOrEmpty(this.ByScope) ?
            $@"By.XPath(@"".//{this.Element.Name}[{this.ByScope}]"")" :
            $@"By.XPath(@"".//{this.Element.Name}"")";
    }
}
