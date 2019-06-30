using src.allors.material.apps.objects.person.create;
using src.allors.material.apps.objects.person.list;
using src.allors.material.apps.objects.person.overview;
using src.allors.material.apps.objects.person.overview.detail;

namespace Tests.PersonTests
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class PersonEditTest : Test
    {
        private readonly PersonListComponent people;

        public PersonEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new People(this.Session).Extent().ToArray();

            var personCreate = this.people.CreatePerson();

            personCreate
                .Salutation.Set(new Salutations(this.Session).Mr.Name)
                .FirstName.Set("Jos")
                .MiddleName.Set("de")
                .LastName.Set("Smos")
                .Function.Set("CEO")
                .Gender.Set(new GenderTypes(this.Session).Male.Name)
                .Locale.Set(this.Session.GetSingleton().AdditionalLocales.First.Name)
                .SAVE.Click();

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

            var person = before.First(v => v.PartyName.Equals("John Doe"));
            var id = person.Id;

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);
            var personOverviewDetail = personOverview.PersonOverviewDetail.Click();

            personOverviewDetail.Salutation.Set(new Salutations(this.Session).Mr.Name)
                .FirstName.Set("Jos")
                .MiddleName.Set("de")
                .LastName.Set("Smos")
                .Function.Set("CEO")
                .Gender.Set(new GenderTypes(this.Session).Male.Name)
                .Locale.Set(this.Session.GetSingleton().AdditionalLocales.First.Name)
                .Comment.Set("unpleasant person")
                .SAVE.Click();

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
