namespace Autotest.Testers
{
    using Autotest.Html;

    public partial class ButtonTester : Tester
    {
        public ButtonTester(Element element)
            : base(element)
        {
        }

        public string InnerText => this.Element.InnerText.Trim().EmptyToNull();

        public override string PropertyName => this.Value.ToAlphaNumeric().Capitalize();

        public string Kind
        {
            get
            {
                if (this.InnerText != null)
                {
                    return "InnerText";
                }

                return "Default";
            }
        }

        public string Value
        {
            get
            {
                if (this.InnerText != null)
                {
                    return this.InnerText;
                }

                return string.Empty;
            }
        }
    }
}
