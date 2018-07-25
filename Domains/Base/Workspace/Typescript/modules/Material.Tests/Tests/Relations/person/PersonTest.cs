namespace Intranet.Tests.Relations
{
    using System.Linq;

    using Allors.Domain;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PersonTest : Test
    {
        private readonly PeopleOverviewPage people;

        public PersonTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            this.people.AddNew.Click();
            Assert.Equal("Person", this.Driver.Title);
        }

        [Fact]
        public void Add()
        {
            this.people.AddNew.Click();
            var before = new People(this.Session).Extent().ToArray();

            var page = new PersonPage(this.Driver);
            
            page.FirstName.Value = "Jos";
            page.LastName.Value = "Smos";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var person = after.Except(before).First();

            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("Smos", person.LastName);
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
