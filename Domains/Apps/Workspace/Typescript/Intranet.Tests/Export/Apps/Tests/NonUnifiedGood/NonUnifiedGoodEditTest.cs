namespace Tests.NonUnifiedGood
{
    using System.Linq;

    using Allors.Domain;

    using Angular;

    using Pages.NonUnifiedGood;
    using Pages.ProductTest;

    using Xunit;

    [Collection("Test collection")]
    public class NonUnifiedGoodEditTest : Test
    {
        private readonly ProductListPage goods;

        public NonUnifiedGoodEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.goods = dashboard.Sidenav.NavigateToProductList();
        }

        [Fact]
        public void Create()
        {
            this.goods.AddNew.Click();
            var before = new NonUnifiedGoods(this.Session).Extent().ToArray();

            var page = new NonUnifiedGoodEditPage(this.Driver);

            page.Name.Set("Mercedes Vito")
                .Description.Set("Vans. Born to run.")
                .Part.Set("finished good")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new NonUnifiedGoods(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var good = after.Except(before).First();

            Assert.Equal("Mercedes Vito", good.Name);
            Assert.Equal("Vans. Born to run.", good.Description);
            Assert.Equal("finished good", good.Part.Name);
        }

        [Fact]
        public void Edit()
        {
            //var before = new People(this.Session).Extent().ToArray();

            //var person = before.First(v => v.PartyName.Equals("contact1"));

            //var personOverview = this.people.Select(person);
            //var page = personOverview.Edit();
            
            //.Salutation.Set("Mr.";

            //.FirstName.Text = "Jos";
            //.LastName.Text = "Smos";
            //.Comment.Text = "This is a comment";

            //.Save.Click();

            //this.Session.Rollback();

            //var after = new People(this.Session).Extent().ToArray();

            //Assert.Equal(after.Length, before.Length);

            //Assert.Equal("Mr.", person.Salutation.Name);
            //Assert.Equal("Jos", person.FirstName);
            //Assert.Equal("Smos", person.LastName);
            //Assert.Equal("This is a comment", person.Comment);
        }
    }
}
