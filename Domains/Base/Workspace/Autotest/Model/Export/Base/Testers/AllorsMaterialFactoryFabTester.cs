namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Autotest.Html;

    public partial class AllorsMaterialFactoryFabTester : Tester
    {
        public AllorsMaterialFactoryFabTester(Element element)
            : base(element)
        {
        }

        public override string PropertyName => "Factory";

        public ObjectType ObjectType
        {
            get
            {
                var objectTypeAttributeValue = this.ObjectTypeAttributeValue;
                if (objectTypeAttributeValue != null)
                {
                    var parts = objectTypeAttributeValue.Split('.');
                    var objectTypeName = parts[parts.Length - 1];

                    var metaPopulation = this.Element.Template.Directive.Project.Model.MetaPopulation;
                    var objectType = metaPopulation.Composites.FirstOrDefault(v => string.Equals(v.Name, objectTypeName, StringComparison.OrdinalIgnoreCase));

                    return objectType;
                }

                return null;
            }
        }

        public string Selector => $@"By.XPath(@""//{this.Element.Name}[{this.ByScope}]"")";

        private string ObjectTypeAttributeValue => this.Element.Attributes.FirstOrDefault(v => string.Equals(v.Name, "[objectType]", StringComparison.OrdinalIgnoreCase))?.Value;
    }
}