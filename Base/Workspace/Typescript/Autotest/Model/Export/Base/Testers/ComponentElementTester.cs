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

        public override string PropertyName => (this.IdAttribute?.Value ?? this.NameAttribute?.Value ?? this.Element.PropertyName).Capitalize();

        public string Selector
        {
            get
            {
                if (this.IdAttribute != null)
                {
                    return $"By.Id({this.PropertyName})";
                }

                if (this.NameAttribute != null)
                {
                    return $@"By.XPath(@""//{this.Element.Name}[@{this.NameAttribute.Value}='{this.PropertyName}'{this.ByScopeAnd}]"")";
                }

                return $@"By.XPath(@""//{this.Element.Name}[{this.ByScope}]"")";
            }
        }

        public override string ToString()
        {
            return $"PropertyName[{this.PropertyName}]";
        }
    }
}