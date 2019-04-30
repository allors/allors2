using System;

namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;

    public partial class ComponentElementTester : Tester
    {
        public ComponentElementTester(Element element)
            : base(element)
        {
        }

        public Attribute IdAttribute => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == "id");

        public Attribute NameAttribute => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == "name");

        public override string Name => this.IdAttribute?.Value ?? this.NameAttribute?.Value ?? this.Element.Name;

        public override string Selector
        {
            get
            {
                if (this.IdAttribute != null)
                {
                    return $"By.Id({this.Name})";
                }

                if (this.NameAttribute != null)
                {
                    return $@"By.XPath(@""//{this.Element.Name}[@{this.NameAttribute.Value}='{this.Name}'{this.ByScope}]"")";
                }

                return $@"By.XPath(@""//{this.Element.Name}[{this.ByScope}]"")";
            }
        }
    }
}