namespace Intranet.Tests.Relations
{
    using System.Linq;

    using Allors.Domain;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PersonTest : Test
    {
        public PersonTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            var people = dashboard.Sidenav.NavigateToPeople();
            people.AddNew.Click();
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
