namespace Intranet.Tests.RelationsPerson
{
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PersonListTest : Test
    {
        public PersonListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("People", this.Driver.Title);
        }
    }
}
