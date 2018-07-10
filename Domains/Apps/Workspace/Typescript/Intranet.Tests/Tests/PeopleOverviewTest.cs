namespace Intranet.Tests
{
    using System.Threading.Tasks;

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

        protected override async Task OnInitAsync()
        {
            await this.Login();
            await this.Page.RouterNavigate("/relations/people");
            await this.Page.WaitForAngularAsync();
        }
    }
}
