namespace Tests.Intranet.PersonTests
{
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    [Collection("Test collection")]
    public class PersonListTest : Test
    {
        private PersonListPage page;

        public PersonListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("People", this.Driver.Title);
        }

        [Fact]
        public void Table()
        {
            var person = new People(this.Session).FindBy(M.Person.FirstName, "John0");
            var row = this.page.Table.FindRow(person);
            var cell = row.FindCell("name");

            Assert.Equal("John0 Doe0", cell.Element.Text);
        }
    }
}
