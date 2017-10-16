//------------------------------------------------------------------------------------------------- 
// <copyright file="EmailCommunicationTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Meta;

    using Xunit;

    
    public class EmailCommunicationTests : DomainTest
    {
        [Fact]
        public void GivenEmailCommunicationIsBuild_WhenDeriving_ThenStatusIsSet()
        {
            var originatorEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("originator@allors.com").Build();
            var addresseeEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee@allors.com").Build();

            var communication = new EmailCommunicationBuilder(this.Session)
                .WithOwner(new People(this.Session).FindBy(M.Person.UserName, Users.AdministratorUserName))
                .WithSubject("Hello")
                .WithDescription("Hello world!")
                .WithOriginator(originatorEmail)
                .WithAddressee(addresseeEmail)
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            Assert.Equal(communication.CommunicationEventState, new CommunicationEventStates(this.Session).Scheduled);
            Assert.Equal(communication.CommunicationEventState, communication.LastCommunicationEventState);
        }

        [Fact]
        public void GivenEmailCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").WithPersonRole(new PersonRoles(this.Session).Employee).Build();

            var personalEmailAddress = new ContactMechanismPurposes(this.Session).PersonalEmailAddress;

            var originatorEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("originator@allors.com").Build();
            var originatorContact = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(originatorEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var originator = new PersonBuilder(this.Session).WithLastName("originator").WithPartyContactMechanism(originatorContact).WithPersonRole(new PersonRoles(this.Session).Contact).Build();

            var addresseeEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee@allors.com").Build();
            var addresseeContact = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(addresseeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var addressee = new PersonBuilder(this.Session).WithLastName("addressee").WithPartyContactMechanism(addresseeContact).WithPersonRole(new PersonRoles(this.Session).Contact).Build();

            var carbonCopeeEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("carbonCopee@allors.com").Build();
            var carbonCopeeContact = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(carbonCopeeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var carbonCopee = new PersonBuilder(this.Session).WithLastName("carbon copee").WithPartyContactMechanism(carbonCopeeContact).WithPersonRole(new PersonRoles(this.Session).Contact).Build();

            var blindCopeeEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("blindCopee@allors.com").Build();
            var blindCopeeContact = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(blindCopeeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var blindCopee = new PersonBuilder(this.Session).WithLastName("blind copee").WithPartyContactMechanism(blindCopeeContact).WithPersonRole(new PersonRoles(this.Session).Contact).Build();

            this.Session.Derive();
            this.Session.Commit();

            var communication = new EmailCommunicationBuilder(this.Session)
                .WithSubject("Hello")
                .WithDescription("Hello world!")
                .WithOwner(owner)
                .WithOriginator(originatorEmail)
                .WithAddressee(addresseeEmail)
                .WithCarbonCopy(carbonCopeeEmail)
                .WithBlindCopy(blindCopeeEmail)
                .Build();

            this.Session.Derive();

            Assert.Equal(5, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(originator, communication.InvolvedParties);
            Assert.Contains(addressee, communication.InvolvedParties);
            Assert.Contains(carbonCopee, communication.InvolvedParties);
            Assert.Contains(blindCopee, communication.InvolvedParties);
        }

        [Fact]
        public void GivenEmailCommunication_WhenOriginatorIsDeleted_ThenCommunicationEventIsDeleted()
        {
            var personalEmailAddress = new ContactMechanismPurposes(this.Session).PersonalEmailAddress;

            var originatorEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("originator@allors.com").Build();
            var originatorContact = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(originatorEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var originator = new PersonBuilder(this.Session).WithLastName("originator").WithPartyContactMechanism(originatorContact).WithPersonRole(new PersonRoles(this.Session).Contact).Build();

            var addresseeEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee@allors.com").Build();
            var addresseeContact = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(addresseeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var addressee = new PersonBuilder(this.Session).WithLastName("addressee").WithPartyContactMechanism(addresseeContact).WithPersonRole(new PersonRoles(this.Session).Contact).Build();

            this.Session.Derive();
            this.Session.Commit();

            var communication = new EmailCommunicationBuilder(this.Session)
                .WithSubject("Hello")
                .WithDescription("Hello world!")
                .WithOriginator(originatorEmail)
                .WithAddressee(addresseeEmail)
                .Build();

            this.Session.Derive();

            Assert.Equal(1, this.Session.Extent<EmailCommunication>().Count);

            originator.Delete();
            this.Session.Derive();

            Assert.Equal(0, this.Session.Extent<EmailCommunication>().Count);
        }
    }
}