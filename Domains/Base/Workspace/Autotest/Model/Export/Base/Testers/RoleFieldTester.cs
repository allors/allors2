namespace Autotest.Testers
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Autotest.Html;

    public partial class RoleFieldTester : Tester
    {
        public RoleFieldTester(Element element)
            : base(element)
        {
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

                    return roleType;
                }

                return null;
            }
        }

        private string RoleTypeAttributeValue => this.Element.Attributes.FirstOrDefault(v => string.Equals(v.Name, "[roleType]", StringComparison.OrdinalIgnoreCase))?.Value;

        public override string PropertyName => this.RoleType?.PropertyName;
    }
}