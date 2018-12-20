namespace Tests.Intranet.PersonTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Tests.Components;
    using Tests.Intranet.ProductTest;

    using Xunit;

    [Collection("Test collection")]
    public class GoodEditTest : Test
    {
        private readonly ProductListPage goods;

        public GoodEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.goods = dashboard.Sidenav.NavigateToProductList();
        }

        [Fact]
        public void Add()
        {
            this.goods.AddNew.Click();
            var before = new Goods(this.Session).Extent().ToArray();

            var page = new GoodEditPage(this.Driver);

            page.Name.Value = "Mercedes Vito";
            page.Description.Value = "Vans. Born to run.";
            page.Part.Value = "finished good";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Goods(this.Session).Extent().ToArray();

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
            
            //page.Salutation.Value = "Mr.";

            //page.FirstName.Text = "Jos";
            //page.LastName.Text = "Smos";
            //page.Comment.Text = "This is a comment";

            //page.Save.Click();

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
