// <copyright file="RoleFieldTester.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Allors.Workspace.Meta;
    using Autotest.Html;
    using Humanizer;

    public partial class RoleFieldTester : Tester
    {
        private const string IsAMatStaticKey = "IsAMatStatic";
        private const string NameAttribute = "attr.data-allors-name";

        public RoleFieldTester(Element element)
            : base(element)
        {
        }

        public string Type
        {
            get
            {
                var parts = this.Element.Name.Split('-')
                    .Skip(1)
                    .Select(v => v.Dehumanize());
                return string.Concat(parts);
            }
        }

        public RoleType RoleType
        {
            get
            {
                var roleTypeAttributeValue = this.RoleTypeAttributeValue;
                if (roleTypeAttributeValue != null)
                {
                    var parts = roleTypeAttributeValue.Split('.');
                    var objectTypeName = parts[parts.Length - 2];
                    var roleTypeName = parts[parts.Length - 1];

                    var metaPopulation = this.Element.Template.Directive.Project.Model.MetaPopulation;

                    var objectType = metaPopulation.Composites.FirstOrDefault(v => string.Equals(v.Name, objectTypeName, StringComparison.OrdinalIgnoreCase));
                    var roleType = objectType?.RoleTypes.FirstOrDefault(v => string.Equals(v.PropertyName, roleTypeName, StringComparison.OrdinalIgnoreCase));

                    if (roleType == null)
                    {
                        throw new Exception($"Could not find RoleType for {roleTypeAttributeValue}");
                    }

                    return roleType;
                }

                return null;
            }
        }

        public string NameAttributeValue => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == NameAttribute)?.Value;

        public override string PropertyName
        {
            get
            {
                if (this.Element.Template.Directive?.Type?.Name == "ProductQuoteCreateComponent" && this.RoleType.PropertyName == "Comment")
                {
                    Console.WriteLine();
                }

                if (this.NameAttributeValue != null)
                {
                    return this.NameAttributeValue;
                }

                if (this.RoleType != null)
                {
                    var propertyName = this.RoleType.PropertyName;

                    var samePropertyName = this.Element.Template.Directive.Testers
                        .OfType<RoleFieldTester>()
                        .Any(v =>
                        {
                            if (v != this)
                            {
                                if (v.RoleType != null)
                                {
                                    if (v.RoleType.PropertyName == propertyName)
                                    {
                                        if (v.Element.InScope == this.Element.InScope)
                                        {
                                            if (!v[IsAMatStaticKey])
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }

                            return false;
                        });

                    if (samePropertyName)
                    {
                        if (this[IsAMatStaticKey])
                        {
                            return propertyName + "Static";
                        }

                        return this.RoleType.AssociationType.ObjectType.Name + propertyName;
                    }

                    return this.RoleType?.PropertyName;
                }

                return null;
            }
        }

        private string RoleTypeAttributeValue => this.Element.Attributes.FirstOrDefault(v => string.Equals(v.Name, "[roleType]", StringComparison.OrdinalIgnoreCase))?.Value;

        public override string ToString() => $"{base.ToString()} Type[{this.Type}] RoleType[{this.RoleType}] NameAttributeValue[{this.NameAttributeValue}] RoleTypeAttributeValue[{this.RoleTypeAttributeValue}]";
    }
}
