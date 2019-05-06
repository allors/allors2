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

        private const string NameAttribute = "attr.data-allors-name";

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

        public string NameAttributeValue => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == NameAttribute)?.Value;

        private string RoleTypeAttributeValue => this.Element.Attributes.FirstOrDefault(v => string.Equals(v.Name, "[roleType]", StringComparison.OrdinalIgnoreCase))?.Value;

        public override string PropertyName
        {
            get
            {
                if (this.NameAttributeValue != null)
                {
                    return this.NameAttributeValue;
                }

                if (this.RoleType != null)
                {
                    var samePropertyName = this.Element.Template.Directive.Testers
                        .OfType<RoleFieldTester>()
                        .Any(v => v != this &&
                                  v.Element.InScope == this.Element.InScope &&
                                  v.RoleType != null &&
                                  v.RoleType.PropertyName == this.RoleType.PropertyName &&
                                  v.RoleType.AssociationType.ObjectType != this.RoleType.AssociationType.ObjectType);

                    if (samePropertyName)
                    {
                        var fullPropertyName = this.RoleType.AssociationType.ObjectType.Name + this.RoleType.PropertyName;
                        return fullPropertyName;
                    }

                    return this.RoleType?.PropertyName;
                }

                return null;
            }
        }
    }
}