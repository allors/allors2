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

        public string FormControl => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == FormControlAttribute)?.Value;

        public string NgModel => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == NgModelAttribute)?.Value;

        public override string Name
        {
            get
            {
                var name = this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == NameAttribute)?.Value;
                return (name ?? this.FormControlName ?? this.FormControl ?? this.NgModel.ToAlphaNumeric()).Capitalize();
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

                if (this.FormControl != null)
                {
                    return "FormControl";
                }

                if (this.NgModel != null)
                {
                    return "NgModel";
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

                if (this.FormControl != null)
                {
                    return this.FormControl;
                }

                if (this.NgModel != null)
                {
                    return this.NgModel;
                }

                return string.Empty;
            }
        }
    }
}