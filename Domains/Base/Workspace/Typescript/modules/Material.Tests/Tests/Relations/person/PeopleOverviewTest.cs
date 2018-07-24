namespace Intranet.Tests.Relations
{
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PeopleOverviewTest : Test
    {
        public PeopleOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("People", this.Driver.Title);
        }
    }
}
