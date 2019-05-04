using System;

namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;

    public partial class InputTester : Tester
    {
        private const string IdAttribute = "id";

        private const string NameAttribute = "name";

        private static readonly string FormControlNameAttribute = "formControlName".ToLowerInvariant();

        public InputTester(Element element)
            : base(element)
        {
        }

        public string IdAttributeValue => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == IdAttribute)?.Value;

        public string NameAttributeValue => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == NameAttribute)?.Value;

        public string FormControlNameAttributeValue => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == FormControlNameAttribute)?.Value;

        public override string PropertyName => (this.IdAttributeValue ?? this.NameAttributeValue ?? this.FormControlNameAttributeValue ?? "Input").Capitalize();

        public string Kind
        {
            get
            {
                if (this.IdAttributeValue != null)
                {
                    return "Id";
                }

                if (this.NameAttributeValue != null)
                {
                    return "Name";
                }

                if (this.FormControlNameAttributeValue != null)
                {
                    return "FormControlName";
                }

                return "Default";
            }
        }

        public string Value => this.IdAttributeValue ?? this.NameAttributeValue ?? this.FormControlNameAttributeValue ?? string.Empty;
    }
}