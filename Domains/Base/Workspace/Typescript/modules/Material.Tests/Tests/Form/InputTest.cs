namespace Tests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Angular;
    using Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class InputTest : Test
    {
        private readonly FormPage page;

        public InputTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToForm();
        }
        
        [Fact]
        public void Initial()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.String.Value = "Hello";

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Equal("Hello", data.String);
        }
    }
}
