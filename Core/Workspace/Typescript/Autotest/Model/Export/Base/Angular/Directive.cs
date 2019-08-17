// <copyright file="Directive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Autotest.Angular
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Autotest.Html;
    using Autotest.Testers;
    using Autotest.Typescript;
    using Newtonsoft.Json.Linq;

    public partial class Directive
    {
        private static readonly Regex[] StringRegexes =
        {
            new Regex(@"^'(.*)'$"),
            new Regex(@"^""(.*)""$"),
        };

        public Dictionary<Element, Directive> ComponentByElement { get; set; }

        public Module DeclaringModule => this.Project.Modules.First(v => v.DeclaredDirectives.Contains(this));

        public Dictionary<Element, Directive[]> AttributeDirectivesByElement { get; set; }

        public Element[] FlattenedElements { get; private set; }

        public Element[] FlattenedElementsWithDirectives { get; set; }

        public string ExportAs { get; set; }

        public bool IsComponent { get; set; }

        public JToken Json { get; set; }

        public Project Project { get; set; }

        public Reference Reference { get; set; }

        public string Selector { get; set; }

        public Template Template { get; set; }

        public Tester[] Testers { get; set; }

        public Class Type { get; set; }

        public string Scope { get; set; }

        public void BaseLoad()
        {
            this.IsComponent = this.Json["isComponent"]?.Value<bool>() ?? false;
            this.Selector = this.Json["selector"]?.Value<string>();
            this.ExportAs = this.Json["exportAs"]?.Value<string>();

            var jsonTemplate = this.Json["template"];
            this.Template = jsonTemplate != null ? new Template(this, jsonTemplate) : null;
            this.Template?.BaseLoad();

            var typeTemplate = this.Json["type"];
            this.Type = typeTemplate != null ? new Class(this, typeTemplate) : null;
            this.Type?.BaseLoad();
        }

        public void BaseLoadTemplate()
        {
            if (this.Template != null)
            {
                this.FlattenedElements = this.Template.FlattenedElements;
                this.ComponentByElement = this.FlattenedElements
                    .Select(v => new
                    {
                        element = v,
                        component = this.DeclaringModule.LookupComponent(v),
                    })
                    .Where(v => v.component != null)
                    .ToDictionary(v => v.element, v => v.component);

                this.AttributeDirectivesByElement = this.FlattenedElements
                    .Select(v => new
                    {
                        element = v,
                        directives = this.DeclaringModule.LookupAttributeDirectives(v),
                    })
                    .Where(v => v.directives.Length > 0)
                    .ToDictionary(v => v.element, v => v.directives);

                foreach (var kvp in this.ComponentByElement)
                {
                    kvp.Key.Component = kvp.Value;
                }

                foreach (var kvp in this.AttributeDirectivesByElement)
                {
                    kvp.Key.AttributeDirectives = kvp.Value;
                }

                foreach (var element in this.ComponentByElement.Keys.Union(this.AttributeDirectivesByElement.Keys))
                {
                    this.ComponentByElement.TryGetValue(element, out var component);
                    this.AttributeDirectivesByElement.TryGetValue(element, out var attributeDirectives);

                    element.Directives = new[] { component }
                        .Concat(attributeDirectives ?? new Directive[0])
                        .Where(v => v != null)
                        .ToArray();
                }

                this.FlattenedElementsWithDirectives = this.FlattenedElements
                    .Where(v => v.Directives != null && v.Directives.Length > 0)
                    .ToArray();

                this.Testers = this.FlattenedElements
                    .Select(TesterFactory.Create)
                    .Where(v => v != null)
                    .ToArray();

                var rootScope = new Scope { Name = this.Scope };
                foreach (var node in this.Template.Html)
                {
                    node.SetInScope(rootScope);
                }

                foreach (var grouping in this.Testers.GroupBy(v => v.ScopedPropertyName).Where(v => v.Count() > 1))
                {
                    var index = 0;
                    foreach (var item in grouping)
                    {
                        item.Index = ++index;
                    }
                }

                if (this.Type != null)
                {
                    var decorator = "@HostBinding('attr.data-test-scope')";
                    var testScopeProperty = this.Type.Members.OfType<Property>()
                        .FirstOrDefault(v => v.Decorators.Contains(decorator));
                    if (testScopeProperty != null)
                    {
                        var initializer = testScopeProperty.Initializer;
                        if (initializer.Equals("this.constructor.name"))
                        {
                            this.Scope = this.Type.Name;
                        }
                        else
                        {
                            foreach (var regex in StringRegexes)
                            {
                                var match = regex.Match(initializer);
                                if (match.Success)
                                {
                                    this.Scope = match.Groups[1].Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
