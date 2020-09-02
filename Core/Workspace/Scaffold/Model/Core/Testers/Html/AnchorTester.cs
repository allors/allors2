// <copyright file="AnchorTester.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;

    public partial class AnchorTester : Tester
    {
        private static readonly string RouterLinkAttribute = "[routerLink]".ToLowerInvariant();

        public AnchorTester(Element element)
            : base(element)
        {
        }

        public string RouterLink => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == RouterLinkAttribute)?.Value;

        public string InnerText => this.Element.InnerText.Trim().EmptyToNull();

        public override string PropertyName => (this.Value.ToAlphaNumeric() ?? this.RouterLink.ToAlphaNumeric()).Capitalize();

        public string Kind
        {
            get
            {
                if (this.InnerText != null)
                {
                    return "InnerText";
                }

                if (this.RouterLink != null)
                {
                    return "RouterLink";
                }

                return "Default";
            }
        }

        public string Value
        {
            get
            {
                if (this.InnerText != null)
                {
                    return this.InnerText;
                }

                if (this.RouterLink != null)
                {
                    return this.RouterLink;
                }

                return string.Empty;
            }
        }
    }
}
