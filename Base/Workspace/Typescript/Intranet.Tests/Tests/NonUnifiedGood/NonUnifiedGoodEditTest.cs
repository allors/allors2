using src.allors.material.@base.objects.good.list;

namespace Tests.NonUnifiedGood
{
    using System.Linq;

    using Allors.Domain;

    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class NonUnifiedGoodEditTest : Test
    {
        private readonly GoodListComponent goods;

        public NonUnifiedGoodEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.goods = this.Sidenav.NavigateToGoods();
        }

        [Fact]
        public void Create()
        {
            var before = new NonUnifiedGoods(this.Session).Extent().ToArray();
            
            var nonUnifiedGoodCreate = this.goods.CreateNonUnifiedGood();

            nonUnifiedGoodCreate
                .Name.Set("Mercedes Vito")
                .Description.Set("Vans. Born to run.")
                .Part.Set("finished good")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new NonUnifiedGoods(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var good = after.Except(before).First();

            Assert.Equal("Mercedes Vito", good.Name);
            Assert.Equal("Vans. Born to run.", good.Description);
            Assert.Equal("finished good", good.Part.Name);
        }
    }
}
