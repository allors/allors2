namespace Tests.SerialisedItemTests
{
    using Allors.Domain;
    using Allors.Meta;

    using src.allors.material.apps.objects.person.list;

    using Xunit;

    [Collection("Test collection")]
    public class SerialisedItemOverviewTest : Test
    {
        private readonly PersonListComponent people;

        public SerialisedItemOverviewTest(TestFixture fixture)
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
