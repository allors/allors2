using Autotest.Html;

namespace Autotest.Testers
{
    public partial class ButtonTester : Tester
    {
        public ButtonTester(Element element) : base(element)
        {
        }

        public string Name => this.Element.InnerText.RemoveWhitespace();

        public string Value => this.Element.InnerText;
    }
}