namespace Intranet.Tests.RelationsPerson
{
    using System.Linq;

    using Allors.Domain;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PersonTest : Test
    {
        private readonly PersonListPage personListPage;

        public PersonTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.personListPage = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Title()
        {
            this.personListPage.AddNew.Click();
            Assert.Equal("Person", this.Driver.Title);
        }

        [Fact]
        public void Add()
        {
            this.personListPage.AddNew.Click();
            var before = new People(this.Session).Extent().ToArray();

            var page = new PersonPage(this.Driver);

            page.Salutation.Value = "Mr.";

            page.FirstName.Value = "Jos";
            page.LastName.Value = "Smos";
            page.Comment.Value = "This is a comment";

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

        [Fact]
        public void Edit()
        {
            var before = new People(this.Session).Extent().ToArray();

            var person = before.First(v => v.PartyName.Equals("contact1"));

            var personOverview = this.personListPage.Select(person);
            var page = personOverview.Edit();
            
            page.Salutation.Value = "Mr.";

            page.FirstName.Value = "Jos";
            page.LastName.Value = "Smos";
            page.Comment.Value = "This is a comment";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("Mr.", person.Salutation.Name);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("This is a comment", person.Comment);
        }
    }
}
