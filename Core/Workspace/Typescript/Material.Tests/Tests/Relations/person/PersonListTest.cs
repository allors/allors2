using src.allors.material.custom.relations.people;

namespace Tests.Relations
{
    using Allors.Domain;
    using Allors.Meta;



    using Xunit;

    [Collection("Test collection")]
    public class PersonListTest : Test
    {
        private src.allors.material.custom.relations.people.PeopleComponent page;

        public PersonListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToPeople();
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
