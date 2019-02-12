namespace Tests.TelecommunicationsNumberTests
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Tests.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class TelecommunicationsNumberEditTest : Test
    {
        private readonly PersonListPage people;

        private readonly TelecommunicationsNumber editContactMechanism;

        public TelecommunicationsNumberEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            this.editContactMechanism = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("0032")
                .WithAreaCode("498")
                .WithContactNumber("123 456")
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
            var before = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewTelecommunicationsNumber();

            page.ContactPurposes.Toggle(new ContactMechanismPurposes(this.Session).BillingAddress.Name);
            page.CountryCode.Value = "111";
            page.AreaCode.Value = "222";
            page.ContactNumber.Value = "333";
            page.ContactMechanismType.Value = new ContactMechanismTypes(this.Session).MobilePhone.Name;
            page.Description.Value = "description";

            page.Save.Click();

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
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var before = new TelecommunicationsNumbers(this.Session).Extent().ToArray();

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectTelecommunicationsNumber(this.editContactMechanism);

            page.CountryCode.Value = "111";
            page.AreaCode.Value = "222";
            page.ContactNumber.Value = "333";
            page.ContactMechanismType.Value = new ContactMechanismTypes(this.Session).MobilePhone.Name;
            page.Description.Value = "description";

            page.Save.Click();

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
