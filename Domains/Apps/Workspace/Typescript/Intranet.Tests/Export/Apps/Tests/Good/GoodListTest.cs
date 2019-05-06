namespace Tests.ProductTest
{
    using Xunit;

    [Collection("Test collection")]
    public class GoodListTest : Test
    {
        public GoodListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToGoods();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Goods", this.Driver.Title);
        }
    }
}
