// <copyright file="AllorsMaterialFactoryFabTester.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Allors.Workspace.Meta;
    using Autotest.Angular;
    using Autotest.Html;

    public partial class AllorsMaterialFactoryFabTester : Tester
    {
        public AllorsMaterialFactoryFabTester(Element element)
            : base(element)
        {
        }

        public Model Model => this.Element.Template.Directive.Project.Model;

        public override string PropertyName => "Factory";

        public Composite ObjectType
        {
            get
            {
                var objectTypeAttributeValue = this.ObjectTypeAttributeValue;
                if (objectTypeAttributeValue != null)
                {
                    var parts = objectTypeAttributeValue.Split('.');
                    var objectTypeName = parts[parts.Length - 1];

                    var metaPopulation = this.Model.MetaPopulation;
                    var objectType = metaPopulation.Composites.FirstOrDefault(v => string.Equals(v.Name, objectTypeName, StringComparison.OrdinalIgnoreCase));

                    return objectType;
                }

                return null;
            }
        }

        public Factory[] Factories
        {
            get
            {
                var classes = this.ObjectType.Classes.Where(v => this.Model.MetaExtensions[v.Id].Create != null);
                return classes.Select(v => new Factory(this, v)).ToArray();
            }
        }

        public string Selector => !string.IsNullOrEmpty(this.ByScope) ?
            $@"By.XPath(@"".//{this.Element.Name}[{this.ByScope}]"")" :
            $@"By.XPath(@"".//{this.Element.Name}"")";

        private string ObjectTypeAttributeValue => this.Element.Attributes.FirstOrDefault(v => string.Equals(v.Name, "[objectType]", StringComparison.OrdinalIgnoreCase))?.Value;

        public class Factory
        {
            public Factory(AllorsMaterialFactoryFabTester tester, Class @class)
            {
                this.Tester = tester;
                this.Class = @class;
            }

            public AllorsMaterialFactoryFabTester Tester { get; }

            public Class Class { get; }

            public Directive Component
            {
                get
                {
                    var metaExtension = this.Tester.Model.MetaExtensions[this.Class.Id];
                    var component = metaExtension.Create;
                    if (component != null)
                    {
                        return this.Tester.Model.Project.LocalNonRoutedScopedComponentsWithoutSelector.FirstOrDefault(v => v.Type?.Name == component) ??
                               this.Tester.Model.Project.LocalNonRoutedScopedComponents.FirstOrDefault(v => v.Type?.Name == component) ??
                               this.Tester.Model.Project.LocalScopedDirectives.FirstOrDefault(v => v.Type?.Name == component);
                    }

                    return null;
                }
            }
        }
    }
}
