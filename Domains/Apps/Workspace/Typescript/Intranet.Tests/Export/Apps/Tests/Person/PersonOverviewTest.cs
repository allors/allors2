using src.allors.material.apps.objects.person.list;

namespace Tests.PersonTests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    [Collection("Test collection")]
    public class PersonOverviewTest : Test
    {
        private readonly PersonListComponent people;

        public PersonOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John0");
            this.people.Select(person);
            Assert.Equal("Person", this.Driver.Title);
        }

        [Fact]
        public void NavigateToList()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John0");
            var overviewPage = this.people.Select(person);
            Assert.Equal("Person", this.Driver.Title);

            overviewPage.List.Click();

            Assert.Equal("People", this.Driver.Title);
        }
    }
}
