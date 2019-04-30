namespace Autotest.Testers
{
    using System.Linq;
    using Autotest.Html;

    public partial class InputTester : Tester
    {
        public InputTester(Element element)
            : base(element)
        {
        }

        public override string Name => this.Element.Attributes
            .FirstOrDefault(v => v.Name?.ToLowerInvariant() == "formcontrolname")?.Value;

        public override string Selector => $@"By.XPath(@""//input[@formcontrolname='{this.Name}'{this.ByScope}]"")";
    }
}