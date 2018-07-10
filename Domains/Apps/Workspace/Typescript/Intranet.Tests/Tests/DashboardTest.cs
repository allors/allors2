namespace Intranet.Tests
{
    using Xunit;

    [Collection("Test collection")]
    public class DashboardTest : Test
    {
        public DashboardTest(TestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async void Title()
        {
            await this.Login();

            Assert.Equal("Dashboard", await this.Page.GetTitleAsync());
        }
    }
}
