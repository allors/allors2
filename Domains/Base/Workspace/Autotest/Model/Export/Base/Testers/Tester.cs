using System.Linq;
using Autotest.Html;

namespace Autotest.Testers
{
    public abstract partial class Tester
    {
        public Element Element { get; }

        public bool IsInput => this.Element.Name == "input";

        public string Name => this.Element.Attributes.FirstOrDefault(v => v.Name?.ToLowerInvariant() == "formcontrolname")?.Value;
        
        protected Tester(Element element)
        {
            this.Element = element;
        }
    }
}