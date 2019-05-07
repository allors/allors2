using src.allors.material.custom.tests.form;

namespace Tests
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Components;

    using Xunit;

    [Collection("Test collection")]
    public class ChipsTest : Test
    {
        private readonly FormComponent page;

        public ChipsTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }

        [Fact]
        public void AddOne()
        {
            var jane = new People(this.Session).FindBy(M.Person.UserName, "jane@doe.org");

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.Chips.Add("jane", "jane@doe.org");

            this.page.SAVE.Click();

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

            this.page.SAVE.Click();

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

            this.page.SAVE.Click();
            
            this.page.Chips.Remove("jane@doe.org");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var data = after.Except(before).First();

            Assert.Empty(data.Chips);
        }
    }
}
