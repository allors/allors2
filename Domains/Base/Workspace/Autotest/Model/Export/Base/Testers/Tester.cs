namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Autotest.Html;

    public abstract partial class Tester
    {
        protected Tester(Element element)
        {
            this.Element = element;
        }

        public Element Element { get; }

        public abstract string Name { get; }

        public abstract string Selector { get; }

        public string Scope => this.Element.Ancestors.FirstOrDefault(v => v.Scope != null)?.Scope ?? this.Element.Template.Directive.Scope;

        public bool this[string typeCheck] =>
            string.Equals($"is{this.Element.Name}", typeCheck, StringComparison.OrdinalIgnoreCase) ||
            string.Equals($"is{this.GetType().Name}", typeCheck, StringComparison.OrdinalIgnoreCase);
    }
}