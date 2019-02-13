namespace Tests.ElectronicAddressTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Angular;

    using Pages.ElectronicAddressTests;
    using Pages.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class EmailAddressEditTest : Test
    {
        private readonly PersonListPage personListPage;

        public EmailAddressEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.personListPage = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var before = new EmailAddresses(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var page = this.personListPage.Select(person).NewEmailAddress();

            page.ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).BillingAddress.Name)
                .ElectronicAddressString.Set("me@myself.com")
                .Description.Set("description")
                .Save.Click();

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
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var electronicAddress = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("info@acme.com")
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(electronicAddress).Build();
            person.AddPartyContactMechanism(partyContactMechanism);

            this.Session.Derive();
            this.Session.Commit();

            var before = new EmailAddresses(this.Session).Extent().ToArray();

            var page = this.personListPage.Select(person).SelectElectronicAddress(electronicAddress);

            page.ElectronicAddressString.Set("me@myself.com")
                .Description.Set("description")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailAddresses(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal("me@myself.com", electronicAddress.ElectronicAddressString);
            Assert.Equal("description", electronicAddress.Description);
        }
    }
}
