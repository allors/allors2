namespace Intranet.Tests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class TextAreaTest : Test
    {
        private readonly FormPage page;

        public TextAreaTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToForm();
        }
        
        [Fact]
        public void Initial()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.TextArea.Value = "Hello";

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal("Hello", data.TextArea);
        }
    }
}
