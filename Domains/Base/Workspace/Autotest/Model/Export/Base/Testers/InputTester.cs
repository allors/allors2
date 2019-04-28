using System.Linq;
using Autotest.Html;

namespace Autotest.Testers
{
    public partial class InputTester : Tester
    {
        public InputTester(Element element) : base(element)
        {
        }
        
        public string Name => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == "formcontrolname")?.Value;
    }
}