using Autotest.Html;

namespace Autotest.Testers
{
    public static partial class TesterFactory
    {
        public static Tester Create(Element element)
        {
            return CustomCreate(element) ?? BaseCreate(element);
        }
    }
}