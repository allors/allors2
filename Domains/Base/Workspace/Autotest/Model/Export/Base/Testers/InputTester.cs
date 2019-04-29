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

        public override string Selector => this.Scope != null
            ? $@"By.XPath(@""//input[@formcontrolname='{this.Name}' and ancestor::*[@data-test-scope][1]/@data-test-scope='{this.Scope}']"")"
            : $@"By.XPath(@""//input[@formcontrolname='{this.Name}']"")";
    }
}