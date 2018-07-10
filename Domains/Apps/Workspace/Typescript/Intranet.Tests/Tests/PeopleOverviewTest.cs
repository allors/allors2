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
        }

        [Fact]
        public async void Title()
        {
            Assert.Equal("People", await this.Page.GetTitleAsync());
        }

        [Fact]
        public async void Search()
        {
            var page = new PeopleOverviewPage(this.Page);

            await page.LastName.TypeAsync("jos");
            var value = await page.LastName.Value();

            Assert.Equal("jos", value);
        }


        protected override async Task OnInitAsync()
        {
            await this.Login();
            await this.NavigateByUrl("/relations/people");
        }
    }
}
