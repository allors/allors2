namespace Intranet.Tests.RelationsPerson
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PersonOverviewTest : Test
    {
        private readonly PersonListPage people;

        public PersonOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            this.people.Select(person);
            Assert.Equal("Person Overview", this.Driver.Title);
        }
    }
}
