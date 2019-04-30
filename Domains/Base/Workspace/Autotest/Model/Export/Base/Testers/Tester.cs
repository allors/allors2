namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using System.Text;
    using Autotest.Html;

    public abstract partial class Tester
    {
        protected Tester(Element element)
        {
            this.Element = element;
        }

        public Element Element { get; }

        public abstract string Name { get; }

        public string PropertyName => this.ElementScopes.Aggregate(this.Name.Capitalize(), (current, scope) => scope.Capitalize() + "_" + current);

        public string[] ElementScopes => this.Element.Ancestors.Where(v => v.Scope != null).Select(v => v.Scope).ToArray();

        public string[] Scopes => this.Element.Template.Directive.Scope != null ?
            this.ElementScopes.Concat(new[] { this.Element.Template.Directive.Scope }).ToArray() :
            this.ElementScopes;

        public string ByScope
        {
            get
            {
                var byScope = new StringBuilder();
                var scopes = this.Scopes;
                if (scopes.Length > 0)
                {
                    for (var i = 0; i < scopes.Length; i++)
                    {
                        var scope = scopes[i];
                        var index = i + 1;
                        byScope.Append($" and ancestor::*[@data-test-scope][{index}]/@data-test-scope='{scope}'");
                    }
                }

                return byScope.ToString();
            }
        }

        public bool this[string typeCheck]
        {
            get
            {
                var typeName = this.GetType().Name;
                var componentName = this.Element.Component?.Type?.Name;
                var elementName = this.Element.Name;

                return string.Equals($"is{typeName}", typeCheck, StringComparison.OrdinalIgnoreCase) ||
                       (componentName != null && string.Equals($"is{componentName}", typeCheck, StringComparison.OrdinalIgnoreCase)) ||
                       string.Equals($"is{elementName}", typeCheck, StringComparison.OrdinalIgnoreCase);

            }
        }
    }
}