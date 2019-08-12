using src.allors.material.apps.objects.contactmechanism.overview.panel;
using src.allors.material.apps.objects.person.list;
using src.allors.material.apps.objects.person.overview;
using src.allors.material.apps.objects.telecommunicationsnumber.create;
using src.allors.material.apps.objects.telecommunicationsnumber.edit;

namespace Tests.TelecommunicationsNumberTests
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Components;
    using Tests.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class TelecommunicationsNumberEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly TelecommunicationsNumber editContactMechanism;

        public TelecommunicationsNumberEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John Doe"));

            this.editContactMechanism = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("0032")
                .WithAreaCode("498")
                .WithContactNumber("123 456")
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
            var before = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John Doe"));

            this.people.Table.DefaultAction(person);
            new PersonOverviewComponent(this.people.Driver).ContactmechanismOverviewPanel.Click().CreateTelecommunicationsNumber();

            var createComponent = new TelecommunicationsNumberCreateComponent(this.Driver);
            createComponent
                .ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber.Name)
                .CountryCode.Set("111")
                .AreaCode.Set("222")
                .ContactNumber.Set("333")
                .ContactMechanismType.Set(new ContactMechanismTypes(this.Session).MobilePhone.Name)
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var contactMechanism = after.Except(before).First();

            Assert.Equal("111", contactMechanism.CountryCode);
            Assert.Equal("222", contactMechanism.AreaCode);
            Assert.Equal("333", contactMechanism.ContactNumber);
            Assert.Equal(new ContactMechanismTypes(this.Session).MobilePhone, contactMechanism.ContactMechanismType);
            Assert.Equal("description", contactMechanism.Description);
        }

        [Fact]
        public void Edit()
        {
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John Doe"));

            var before = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);

            var contactMechanismOverview = personOverview.ContactmechanismOverviewPanel.Click();
            var row = contactMechanismOverview.Table.FindRow(this.editContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            var editComponent = new TelecommunicationsNumberEditComponent(this.Driver);
            editComponent
                .CountryCode.Set("111")
                .AreaCode.Set("222")
                .ContactNumber.Set("333")
                .ContactMechanismType.Set(new ContactMechanismTypes(this.Session).MobilePhone.Name)
                .Description.Set("description")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("111", this.editContactMechanism.CountryCode);
            Assert.Equal("222", this.editContactMechanism.AreaCode);
            Assert.Equal("333", this.editContactMechanism.ContactNumber);
            Assert.Equal(new ContactMechanismTypes(this.Session).MobilePhone, this.editContactMechanism.ContactMechanismType);
            Assert.Equal("description", this.editContactMechanism.Description);
        }
    }
}
