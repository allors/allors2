namespace Tests.Remote
{
    using NUnit.Framework;

    using OpenQA.Selenium;

    using Should;

    public class UnitTests : Test
    {
        [Test]
        public void Status()
        {
            var wrappedDriver = this.Driver.WrappedDriver;
            wrappedDriver.Navigate().GoToUrl(Test.UnitTestsUrl);

            var resultDiv = wrappedDriver.FindElement(By.Id("result"));
            var bad = resultDiv.FindElement(By.ClassName("bad"));

            var message = resultDiv.Text;
            bad.Text.ShouldEqual("0", message);
        }
    }
}