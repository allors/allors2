namespace Intranet.Tests
{
    using System.Linq;
    using System.Threading;

    using Allors.Domain;

    using Intranet.Pages;

    using Xunit;

    [Collection("Test collection")]
    public class PersonTest : Test
    {
        public PersonTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/person");
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Person", this.Driver.Title);
        }

        [Fact]
        public void Save()
        {
            var before = new People(this.Session).Extent().ToArray();

            var page = new PersonPage(this.Driver);

            this.Driver.WaitForAngular();

            page.Salutation.Value = "Mr.";

            page.FirstName.Text = "Jos";
            page.LastName.Text = "Smos";
            page.Comment.Text = "This is a comment";

            Assert.Equal("Mr.", page.Salutation.Value);

            page.Save.Click();

            this.Driver.WaitForAngular();

            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var person = after.Except(before).First();

            Assert.Equal("Mr.", person.Salutation.Name);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("This is a comment", person.Comment);
        }
    }
}
