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
            var originatorEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("originator@allors.com").Build();
            var addresseeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("addressee@allors.com").Build();

            var communication = new EmailCommunicationBuilder(this.DatabaseSession)
                .WithOwner(new People(this.DatabaseSession).FindBy(M.Person.UserName, Users.AdministratorUserName))
                .WithSubject("Hello")
                .WithDescription("Hello world!")
                .WithOriginator(originatorEmail)
                .WithAddressee(addresseeEmail)
                .Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            Assert.Equal(communication.CommunicationEventState, new CommunicationEventStates(this.DatabaseSession).Scheduled);
            Assert.Equal(communication.CommunicationEventState, communication.LastCommunicationEventState);
        }

        [Fact]
        public void GivenEmailCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();

            var personalEmailAddress = new ContactMechanismPurposes(this.DatabaseSession).PersonalEmailAddress;

            var originatorEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("originator@allors.com").Build();
            var originatorContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(originatorEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var originator = new PersonBuilder(this.DatabaseSession).WithLastName("originator").WithPartyContactMechanism(originatorContact).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            var addresseeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("addressee@allors.com").Build();
            var addresseeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(addresseeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var addressee = new PersonBuilder(this.DatabaseSession).WithLastName("addressee").WithPartyContactMechanism(addresseeContact).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            var carbonCopeeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("carbonCopee@allors.com").Build();
            var carbonCopeeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(carbonCopeeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var carbonCopee = new PersonBuilder(this.DatabaseSession).WithLastName("carbon copee").WithPartyContactMechanism(carbonCopeeContact).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            var blindCopeeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("blindCopee@allors.com").Build();
            var blindCopeeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(blindCopeeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var blindCopee = new PersonBuilder(this.DatabaseSession).WithLastName("blind copee").WithPartyContactMechanism(blindCopeeContact).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var communication = new EmailCommunicationBuilder(this.DatabaseSession)
                .WithSubject("Hello")
                .WithDescription("Hello world!")
                .WithOwner(owner)
                .WithOriginator(originatorEmail)
                .WithAddressee(addresseeEmail)
                .WithCarbonCopy(carbonCopeeEmail)
                .WithBlindCopy(blindCopeeEmail)
                .Build();

            this.DatabaseSession.Derive();

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
            var personalEmailAddress = new ContactMechanismPurposes(this.DatabaseSession).PersonalEmailAddress;

            var originatorEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("originator@allors.com").Build();
            var originatorContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(originatorEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var originator = new PersonBuilder(this.DatabaseSession).WithLastName("originator").WithPartyContactMechanism(originatorContact).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            var addresseeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("addressee@allors.com").Build();
            var addresseeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(addresseeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var addressee = new PersonBuilder(this.DatabaseSession).WithLastName("addressee").WithPartyContactMechanism(addresseeContact).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var communication = new EmailCommunicationBuilder(this.DatabaseSession)
                .WithSubject("Hello")
                .WithDescription("Hello world!")
                .WithOriginator(originatorEmail)
                .WithAddressee(addresseeEmail)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.DatabaseSession.Extent<EmailCommunication>().Count);

            originator.Delete();
            this.DatabaseSession.Derive();

            Assert.Equal(0, this.DatabaseSession.Extent<EmailCommunication>().Count);
        }
    }
}