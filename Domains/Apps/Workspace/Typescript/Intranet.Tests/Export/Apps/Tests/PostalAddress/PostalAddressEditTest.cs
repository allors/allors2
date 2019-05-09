using src.allors.material.apps.objects.contactmechanism.overview.panel;
using src.allors.material.apps.objects.person.list;
using src.allors.material.apps.objects.person.overview;
using src.allors.material.apps.objects.postaladdress.edit;

namespace Tests.PostalAddressTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class PostalAddressEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly PostalAddress editContactMechanism;

        public PostalAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            this.editContactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                    .WithLocality("city")
                    .WithPostalCode("1111")
                    .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                    .Build())
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(this.editContactMechanism).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");

            var before = new PostalAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            this.people.Table.DefaultAction(person);
            var postalAddressEditComponent = new PersonOverviewComponent(this.people.Driver).ContactmechanismOverviewPanel.Click().CreatePostalAddress();

            postalAddressEditComponent
                .ContactPurposes.Toggle("General correspondence address")
                .Address1.Set("addressline 1")
                .Address2.Set("addressline 2")
                .Address3.Set("addressline 3")
                .Locality.Set("city")
                .PostalCode.Set("postalcode")
                .Country.Set(country.Name)
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PostalAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();
            var partyContactMechanism = contactMechanism.PartyContactMechanismsWhereContactMechanism.First;

            Assert.Equal("addressline 1", contactMechanism.Address1);
            Assert.Equal("addressline 2", contactMechanism.Address2);
            Assert.Equal("addressline 3", contactMechanism.Address3);
            Assert.Equal("addressline 1", contactMechanism.Address1);
            Assert.Equal("city", contactMechanism.PostalBoundary.Locality);
            Assert.Equal("postalcode", contactMechanism.PostalBoundary.PostalCode);
            Assert.Equal(country, contactMechanism.PostalBoundary.Country);
            Assert.Equal("description", contactMechanism.Description);
        }

        [Fact]
        public void Edit()
        {
            var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "NL");

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var before = new PostalAddresses(this.Session).Extent().ToArray();

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);

            var PanelComponent = personOverview.ContactmechanismOverviewPanel.Click();
            var row = PanelComponent.Table.FindRow(this.editContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            var postalAddressEditComponent = new PostalAddressEditComponent(this.Driver);
            postalAddressEditComponent.Address1.Set("addressline 1")
                .Address2.Set("addressline 2")
                .Address3.Set("addressline 3")
                .Locality.Set("city")
                .PostalCode.Set("postalcode")
                .Country.Set(country.Name)
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PostalAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("addressline 1", this.editContactMechanism.Address1);
            Assert.Equal("addressline 2", this.editContactMechanism.Address2);
            Assert.Equal("addressline 3", this.editContactMechanism.Address3);
            Assert.Equal("addressline 1", this.editContactMechanism.Address1);
            Assert.Equal("city", this.editContactMechanism.PostalBoundary.Locality);
            Assert.Equal("postalcode", this.editContactMechanism.PostalBoundary.PostalCode);
            Assert.Equal(country, this.editContactMechanism.PostalBoundary.Country);
            Assert.Equal("description", this.editContactMechanism.Description);
        }
    }
}
