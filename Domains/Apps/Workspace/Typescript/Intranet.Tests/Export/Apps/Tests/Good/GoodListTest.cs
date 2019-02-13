namespace Tests.ProductTest
{
    using Xunit;

    [Collection("Test collection")]
    public class GoodListTest : Test
    {
        public GoodListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToGoodList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Goods", this.Driver.Title);
        }
    }
}
