namespace Tests.Material.Relations
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Tests.Material.Pages.Relations;

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
            var person = new People(this.Session).FindBy(M.Person.UserName, "john@doe.org");
            this.people.Select(person);
            Assert.Equal("Person Overview", this.Driver.Title);
        }
    }
}
