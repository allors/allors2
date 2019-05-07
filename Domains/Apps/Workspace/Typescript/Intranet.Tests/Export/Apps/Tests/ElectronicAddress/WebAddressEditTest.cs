using src.allors.material.apps.objects.person.list;

namespace Tests.ElectronicAddressTests
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Components;

    using Pages.PersonTests;

    using Tests.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class WebAddressEditTest : Test
    {
        private readonly PersonListComponent personListPage;
        
        public WebAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new WebAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.personListPage.Select(person);
            var page = personOverview.NewWebAddress();

            page.ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).BillingAddress.Name)
                .ElectronicAddressString.Set("wwww.allors.com")
                .Description.Set("description")
                .SAVE.Click();

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

            var editContactMechanism = new WebAddressBuilder(this.Session)
                .WithElectronicAddressString("www.acme.com")
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(editContactMechanism).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            var before = new WebAddresses(this.Session).Extent().ToArray();

            var page = this.personListPage.Select(person).SelectElectronicAddress(editContactMechanism);

            page.ElectronicAddressString.Set("wwww.allors.com")
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new WebAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("wwww.allors.com", editContactMechanism.ElectronicAddressString);
            Assert.Equal("description", editContactMechanism.Description);
        }
    }
}
