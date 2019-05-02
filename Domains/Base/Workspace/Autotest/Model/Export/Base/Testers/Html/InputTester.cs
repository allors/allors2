namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;

    public partial class InputTester : Tester
    {
        private const string NameAttribute = "name";

        private static readonly string FormControlNameAttribute = "formControlName".ToLowerInvariant();

        private static readonly string FormControlAttribute = "[formControl]".ToLowerInvariant();

        private static readonly string NgModelAttribute = "[(ngModel)]".ToLowerInvariant();

        public InputTester(Element element)
            : base(element)
        {
        }

        public string FormControlName => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == FormControlNameAttribute)?.Value;

        public override string Name
        {
            get
            {
                var name = this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == NameAttribute)?.Value;
                return (name ?? this.FormControlName).Capitalize();
            }
        }

        public string Kind
        {
            get
            {
                if (this.FormControlName != null)
                {
                    return "FormControlName";
                }

                return "Default";
            }
        }

        public string Value
        {
            get
            {
                if (this.FormControlName != null)
                {
                    return this.FormControlName;
                }

                return string.Empty;
            }
        }
    }
}