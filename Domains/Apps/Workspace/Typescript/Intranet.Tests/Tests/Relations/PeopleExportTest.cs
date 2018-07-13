namespace Intranet.Tests.Relations
{
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PeopleExportTest : Test
    {
        public PeopleExportTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            var people = dashboard.Sidenav.NavigateToPeople();
            people.Export.Click();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Export People to CSV", this.Driver.Title);
        }
    }
}
