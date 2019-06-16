namespace Tests.ApplicationTests
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
            this.Login();

            Assert.Equal("Dashboard", this.Driver.Title);
        }
    }
}
