namespace Tests.Intranet.ElectronicAddressTests
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
    public class WebAddressEditTest : Test
    {
        private readonly PersonListPage people;

        private readonly ElectronicAddress editContactMechanism;

        public WebAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            this.editContactMechanism = new WebAddressBuilder(this.Session)
                .WithElectronicAddressString("www.acme.com")
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
            var before = new WebAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewWebAddress();

            page.ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).BillingAddress.Name);
            page.ElectronicAddressString.Value = "wwww.allors.com";
            page.Description.Value = "description";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new WebAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();
            
            Assert.Equal("wwww.allors.com", contactMechanism.ElectronicAddressString);
            Assert.Equal("description", contactMechanism.Description);
        }

        [Fact]
        public void Edit()
        {
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var before = new WebAddresses(this.Session).Extent().ToArray();

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectElectronicAddress(this.editContactMechanism);

            page.ElectronicAddressString.Value = "wwww.allors.com";
            page.Description.Value = "description";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new WebAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("wwww.allors.com", this.editContactMechanism.ElectronicAddressString);
            Assert.Equal("description", this.editContactMechanism.Description);
        }
    }
}
