using src.allors.material.apps.objects.person.list;

namespace Tests.PersonTests
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class PersonEditTest : Test
    {
        private readonly PersonListComponent people;

        public PersonEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            this.people.AddNew.Click();
            var before = new People(this.Session).Extent().ToArray();

            var page = new PersonEditPage(this.Driver);

            page.Salutation.Set(new Salutations(this.Session).Mr.Name)
                .FirstName.Set("Jos")
                .MiddleName.Set("de")
                .LastName.Set("Smos")
                .Function.Set("CEO")
                .Gender.Set(new GenderTypes(this.Session).Male.Name)
                .Locale.Set(this.Session.GetSingleton().AdditionalLocales.First.Name)
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var person = after.Except(before).First();

            Assert.Equal(new Salutations(this.Session).Mr, person.Salutation);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("de", person.MiddleName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("CEO", person.Function);
            Assert.Equal(new GenderTypes(this.Session).Male, person.Gender);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, person.Locale);
        }

        [Fact]
        public void Edit()
        {
            var before = new People(this.Session).Extent().ToArray();

            var person = before.First(v => v.PartyName.Equals("John0 Doe0"));
            var id = person.Id;

            var personOverview = this.people.Select(person);
            var page = personOverview.Edit();

            page.Salutation.Set(new Salutations(this.Session).Mr.Name)
                .FirstName.Set("Jos")
                .MiddleName.Set("de")
                .LastName.Set("Smos")
                .Function.Set("CEO")
                .Gender.Set(new GenderTypes(this.Session).Male.Name)
                .Locale.Set(this.Session.GetSingleton().AdditionalLocales.First.Name)
                .Comment.Set("unpleasant person")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            person = after.First(v => v.Id.Equals(id));

            Assert.Equal(new Salutations(this.Session).Mr, person.Salutation);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("de", person.MiddleName);
            Assert.Equal("Smos", person.LastName);
            Assert.Equal("CEO", person.Function);
            Assert.Equal(new GenderTypes(this.Session).Male, person.Gender);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, person.Locale);
            Assert.Equal("unpleasant person", person.Comment);
        }
    }
}
