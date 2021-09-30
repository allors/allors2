// <copyright file="Tester.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Autotest.Html;

    public abstract partial class Tester
    {
        protected Tester(Element element) => this.Element = element;

        public Element Element { get; }

        public int? Index { get; set; }

        public abstract string PropertyName { get; }

        public string ScopedPropertyName => this.ElementScopes.Aggregate(this.PropertyName.Capitalize(), (current, scope) => scope.Capitalize() + "_" + current);

        public string IndexedScopedPropertyName => this.Index.HasValue ? $"{this.ScopedPropertyName}_{this.Index}" : this.ScopedPropertyName;

        public string Name => this.IndexedScopedPropertyName;

        public string[] ElementScopes => this.Element.Ancestors.Where(v => v.Scope != null).Select(v => v.Scope).ToArray();

        public string[] Scopes => this.Element.Template.Directive.Scope != null ?
            this.ElementScopes.Concat(new[] { this.Element.Template.Directive.Scope }).ToArray() :
            this.ElementScopes;

        public string ByScopeAnd => string.Concat(this.Scopes.Select((scope, index) => $" and ancestor::*[@data-test-scope][{index + 1}]/@data-test-scope='{scope}'"));

        public string ByScope => string.Concat(this.Scopes.Select((scope, index) => $"{(index > 0 ? " and " : string.Empty)}ancestor::*[@data-test-scope][{index + 1}]/@data-test-scope='{scope}'"));

        public bool this[string typeCheck]
        {
            get
            {
                var typeName = this.GetType().Name;
                var componentName = this.Element.Component?.Type?.Name;
                var elementName = this.Element.PropertyName;

                return string.Equals($"is{typeName}", typeCheck, StringComparison.OrdinalIgnoreCase) ||
                       (componentName != null && string.Equals($"is{componentName}", typeCheck, StringComparison.OrdinalIgnoreCase)) ||
                       string.Equals($"is{elementName}", typeCheck, StringComparison.OrdinalIgnoreCase);
            }
        }

        public override string ToString() => $"Template[{this.Element.Template.Url}] Element[{this.Element.Name}]";
    }
}
