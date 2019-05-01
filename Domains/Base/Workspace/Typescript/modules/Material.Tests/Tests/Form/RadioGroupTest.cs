using src.allors.material.custom.tests.form;

namespace Tests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;
    

    using Xunit;

    [Collection("Test collection")]
    public class RadioGroupTest : Test
    {
        private readonly FormComponent page;

        public RadioGroupTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToForm();
        }
        
        [Fact]
        [Trait("Category", "Investigate")]
        public void Initial()
        {
            this.Driver.WaitForAngular();

            var jane = new People(this.Session).FindBy(M.Person.UserName, "jane@doe.org");

            var before = new Datas(this.Session).Extent().ToArray();

            this.page.RadioGroup.Select("one");

            this.page.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Datas(this.Session).Extent().ToArray();

            Assert.Equal(before.Length + 1, after.Length);

            var data = after.Except(before).First();

            Assert.Equal("one", data.RadioGroup);
        }
    }
}
