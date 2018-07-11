namespace Intranet.Tests
{
    using System.Threading.Tasks;

    using Intranet.Pages;

    using Xunit;

    [Collection("Test collection")]
    public class PeopleOverviewTest : Test
    {
        public PeopleOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/relations/people");
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("People", this.Driver.Title);
        }

        [Fact]
        public void Search()
        {
            var page = new PeopleOverviewPage(this.Driver);

            page.LastName.Text = "jos";

            Assert.Equal("jos", page.LastName.Text);
        }
    }
}
