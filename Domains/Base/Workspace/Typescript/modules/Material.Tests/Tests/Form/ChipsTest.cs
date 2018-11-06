namespace Tests.Material
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Material.Pages;
    using Tests.Material.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class ChipsTest : Test
    {
        private readonly Sidenav sidenav;

        private readonly FormPage page;

        public ChipsTest(TestFixture fixture)
            : base(fixture)
        {
            this.sidenav = this.Login().Sidenav;
            this.page = this.sidenav.NavigateToForm();
        }

        [Fact]
        public void AddOne()
        {
            var jane = new People(this.Session).FindBy(M.Person.UserName, "jane@doe.org");

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@doe.org");

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Contains(jane, data.Chips);
        }

        [Fact]
        public void AddTwo()
        {
            var jane = new People(this.Session).FindBy(M.Person.UserName, "jane@doe.org");
            var john = new People(this.Session).FindBy(M.Person.UserName, "john@doe.org");

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@doe.org");

            this.page.Chips.Add("john", "john@doe.org");

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Contains(jane, data.Chips);
            Assert.Contains(john, data.Chips);
        }

        [Fact]
        public void RemoveOne()
        {
            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@doe.org");

            this.page.Save.Click();
            
            this.page.Chips.Remove("jane@doe.org");

            this.page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Empty(data.Chips);
        }
    }
}
