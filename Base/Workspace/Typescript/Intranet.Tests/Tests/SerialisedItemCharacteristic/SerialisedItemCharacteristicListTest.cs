namespace Tests.SerialisedItemCharacteristicTest
{
    using Xunit;

    [Collection("Test collection")]
    public class SerialisedItemCharacteristicListTest : Test
    {
        public SerialisedItemCharacteristicListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToCharacteristics();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Product Characteristics", this.Driver.Title);
        }
    }
}
