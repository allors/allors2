namespace Tests
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
        public void Title()
        {
            this.Login();

            Assert.Equal("Dashboard", this.Driver.Title);
        }
    }
}