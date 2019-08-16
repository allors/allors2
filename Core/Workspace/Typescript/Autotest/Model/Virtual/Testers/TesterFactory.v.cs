namespace Autotest.Testers
{
    using Autotest.Html;

    public static partial class TesterFactory
    {
        public static Tester Create(Element element) => CustomCreate(element) ?? BaseCreate(element);
    }
}
