using Allors.Meta;
using src.allors.material.@base.objects.contactmechanism.overview.panel;
using src.allors.material.@base.objects.emailaddress.create;
using src.allors.material.@base.objects.emailaddress.edit;
using src.allors.material.@base.objects.person.overview;

namespace Tests.ElectronicAddressTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Components;
    using Xunit;

    using src.allors.material.@base.objects.person.list;

    [Collection("Test collection")]
    public class EmailAddressEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public EmailAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new EmailAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John Doe"));

            this.personListPage.Table.DefaultAction(person);
            var emailAddressCreate = new PersonOverviewComponent(this.personListPage.Driver).ContactmechanismOverviewPanel.Click().CreateEmailAddress();

            emailAddressCreate
                .ContactPurposes.Toggle("General Phone Number")
                .ElectronicAddressString.Set("me@myself.com")
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();

            Assert.Equal("me@myself.com", contactMechanism.ElectronicAddressString);
            Assert.Equal("description", contactMechanism.Description);
        }

        [Fact]
        public void Edit()
        {
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John Doe"));

            var electronicAddress = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("info@acme.com")
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(electronicAddress).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            var before = new EmailAddresses(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverviewComponent = new PersonOverviewComponent(this.personListPage.Driver);

            var contactMechanismOverviewPanel = personOverviewComponent.ContactmechanismOverviewPanel.Click();
            var row = contactMechanismOverviewPanel.Table.FindRow(electronicAddress);
            var cell = row.FindCell("contact");
            cell.Click();

            var emailAddressEditComponent = new EmailAddressEditComponent(this.Driver);
            emailAddressEditComponent
                .ElectronicAddressString.Set("me@myself.com")
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("me@myself.com", electronicAddress.ElectronicAddressString);
            Assert.Equal("description", electronicAddress.Description);
        }
    }
}
