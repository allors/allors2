namespace Intranet.Tests
{
    using System.Linq;

    using Allors.Domain;

    using Intranet.Pages;

    using Xunit;

    using Task = System.Threading.Tasks.Task;

    [Collection("Test collection")]
    public class PersonTest : Test
    {
        public PersonTest(TestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async void Title()
        {
            Assert.Equal("Person", await this.Page.GetTitleAsync());
        }

        [Fact]
        public async void Save()
        {
            var before = new People(this.Session).Extent().ToArray();
            
            var page = new PersonPage(this.Page);

            await page.Salutation.SelectAsyn("Mr.");
            await page.FirstName.TypeAsync("Jos");
            await page.LastName.TypeAsync("Smos");

            await page.Save.ClickAsync();

            this.Session.Rollback();

            var after = new People(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var person = after.Except(before).First();

            Assert.Equal("Mr.", person.Salutation.Name);
            Assert.Equal("Jos", person.FirstName);
            Assert.Equal("Smos", person.LastName);
        }


        protected override async Task OnInitAsync()
        {
            await this.Login();
            await this.NavigateByUrl("/person");
        }
    }
}
