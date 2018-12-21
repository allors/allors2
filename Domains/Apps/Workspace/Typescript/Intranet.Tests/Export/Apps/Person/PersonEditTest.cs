namespace Tests.Intranet.PersonTests
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;

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
        public void Add()
        {
            this.people.AddNew.Click();
            var before = new People(this.Session).Extent().ToArray();

            var page = new PersonEditPage(this.Driver);

            var acme0 = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");

            page.Salutation.Value = new Salutations(this.Session).Mr.Name;
            page.FirstName.Value = "Jos";
            page.MiddleName.Value = "de";
            page.LastName.Value = "Smos";
            page.Function.Value = "CEO";
            page.Gender.Value = new GenderTypes(this.Session).Male.Name;
            page.Locale.Value = this.Session.GetSingleton().AdditionalLocales.First.Name;

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var person = after.Except(before).First();

            Assert.Equal(new Salutations(this.Session).Mr.Name, person.Salutation.Name);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("de", person.MiddleName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("CEO", person.Function);
            Assert.Equal(new GenderTypes(this.Session).Male.Name, person.Gender.Name);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First.Name, person.Locale.Name);
        }

        [Fact]
        public void Edit()
        {
            var before = new People(this.Session).Extent().ToArray();

            var person = before.First(v => v.PartyName.Equals("John0 Doe0"));
            var id = person.Id;

            var personOverview = this.people.Select(person);
            var page = personOverview.Edit();

            page.Salutation.Value = new Salutations(this.Session).Mr.Name;
            page.FirstName.Value = "Jos";
            page.MiddleName.Value = "de";
            page.LastName.Value = "Smos";
            page.Function.Value = "CEO";
            page.Gender.Value = new GenderTypes(this.Session).Male.Name;
            page.Locale.Value = this.Session.GetSingleton().AdditionalLocales.First.Name;
            page.Comment.Value = "unpleasant person";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            person = after.First(v => v.Id.Equals(id));

            Assert.Equal(new Salutations(this.Session).Mr.Name, person.Salutation.Name);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("de", person.MiddleName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("CEO", person.Function);
            Assert.Equal(new GenderTypes(this.Session).Male.Name, person.Gender.Name);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First.Name, person.Locale.Name);
            Assert.Equal("unpleasant person", person.Comment);
        }
    }
}
