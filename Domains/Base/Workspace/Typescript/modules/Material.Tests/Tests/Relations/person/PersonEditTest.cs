namespace Tests.Relations
{
    using System.Linq;

    using Allors.Domain;

    using Pages.Relations;

    using Angular;

    using Xunit;

    [Collection("Test collection")]
    public class PersonEditTest : Test
    {
        private readonly PersonListPage people;

        public PersonEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
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

            var personEditPage = new PersonEditPage(this.Driver);

            personEditPage.FirstName.Set("Jos")
                          .LastName.Set("Smos")
                          .Save.Click();

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
            var before = new People(this.Session).Extent().ToArray();
            var person = before.First(v => v.FullName == "John Doe");

            var page = this.people.Select(person).Edit();

            page.FirstName.Set("Jos")
                .LastName.Set("Smos")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("Smos", person.LastName);
        }
    }
}
