namespace Tests.Intranet.PostalAddressTests
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Intranet.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class PostalAddressEditTest : Test
    {
        private readonly PersonListPage people;

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

            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");

            var before = new PostalAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewPostalAddress();

            page.ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).BillingAddress.Name);
            page.Address1.Value = "addressline 1";
            page.Address2.Value = "addressline 2";
            page.Address3.Value = "addressline 3";
            page.Locality.Value = "city";
            page.PostalCode.Value = "postalcode";
            page.Country.Value = country.Name;
            page.Description.Value = "description";

            page.Save.Click();

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

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectPostalAddress(this.editContactMechanism);

            page.Address1.Value = "addressline 1";
            page.Address2.Value = "addressline 2";
            page.Address3.Value = "addressline 3";
            page.Locality.Value = "city";
            page.PostalCode.Value = "postalcode";
            page.Country.Value = country.Name;
            page.Description.Value = "description";

            page.Save.Click();

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
