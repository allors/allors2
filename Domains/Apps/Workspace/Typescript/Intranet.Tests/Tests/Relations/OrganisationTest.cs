namespace Intranet.Tests.Relations
{
    using System.Linq;
    using Allors.Domain;
    using Intranet.Pages.Relations;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationTest : Test
    {
        public OrganisationTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            var organisations = dashboard.Sidenav.NavigateToOrganisations();
            organisations.AddNew.Click();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Organisation", this.Driver.Title);
        }

        [Fact]
        public void Save()
        {
            var before = new Organisations(this.Session).Extent().ToArray();

            var page = new OrganisationPage(this.Driver);

            this.Driver.WaitForAngular();

            page.Name.Text = "Acme";
            page.TaxNumber.Text = "111.111.111";
            page.IsManufacturer.Value = true;
            page.Comment.Text = "This is a comment";

            page.Save.Click();

            this.Driver.WaitForAngular();

            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var organisation = after.Except(before).First();

            Assert.Equal("Acme", organisation.Name);
            Assert.Equal("111.111.111", organisation.TaxNumber);
            Assert.True(organisation.IsManufacturer);
            Assert.Equal("This is a comment", organisation.Comment);
        }
    }
}
