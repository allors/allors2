using Autotest.Html;

namespace Autotest.Testers
{
    public partial class TesterFactory
    {
        private static Tester BaseCreate(Element element)
        {
            switch (element.Name)
            {
                case "input":
                    return new InputTester(element);
                
                case "button":
                    return new ButtonTester(element);
            }

            return null;
        }
    }
}