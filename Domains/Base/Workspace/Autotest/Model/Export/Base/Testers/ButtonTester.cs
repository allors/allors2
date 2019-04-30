namespace Autotest.Testers
{
    using Autotest.Html;

    public partial class ButtonTester : Tester
    {
        public ButtonTester(Element element)
            : base(element)
        {
        }

        public override string Name => this.Element.InnerText.RemoveWhitespace();

        public string Value => this.Element.InnerText;

        public override string Selector => $@"By.XPath(@""//button[normalize-space()='{this.Value}'{this.ByScope}]"")";
    }
}