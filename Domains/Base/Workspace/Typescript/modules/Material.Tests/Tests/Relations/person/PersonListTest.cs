namespace Tests.Relations
{
    using Allors.Domain;
    using Allors.Meta;

    using Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PersonListTest : Test
    {
        private PeoplePage page;

        public PersonListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("People", this.Driver.Title);
        }

        [Fact]
        public void Table()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John");
            var row = this.page.Table.FindRow(person);
            var cell = row.FindCell("firstName");
            cell.Click();
        }
    }
}
