namespace Tests.Remote
{
    using NUnit.Framework;

    using Should;

    public class MainTests : Test
    {
        [Test]
        public void Navigate()
        {
            this.Driver.Navigate().GoToUrl(Test.AppUrl);
            this.Driver.Title.ShouldEqual("Allors Base");
        }
    }
}