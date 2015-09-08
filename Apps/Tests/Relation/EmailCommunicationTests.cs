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
    using NUnit.Framework;

    [TestFixture]
    public class EmailCommunicationTests : DomainTest
    {
        [Test]
        public void GivenEmailCommunication_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EmailCommunicationBuilder(this.DatabaseSession);
            var communication = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSubject("Email communication");
            communication = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            Assert.AreEqual(communication.CurrentCommunicationEventStatus.CommunicationEventObjectState, new CommunicationEventObjectStates(this.DatabaseSession).Scheduled);
            Assert.AreEqual(communication.CurrentObjectState, new CommunicationEventObjectStates(this.DatabaseSession).Scheduled);
            Assert.AreEqual(communication.CurrentObjectState, communication.LastObjectState);
        }

        [Test]
        public void GivenEmailCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").Build();

            var personalEmailAddress = new ContactMechanismPurposes(this.DatabaseSession).PersonalEmailAddress;

            var originatorEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("originator@allors.com").Build();
            var originatorContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(originatorEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var originator = new PersonBuilder(this.DatabaseSession).WithLastName("originator").WithPartyContactMechanism(originatorContact).Build();

            var addresseeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("addressee@allors.com").Build();
            var addresseeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(addresseeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var addressee = new PersonBuilder(this.DatabaseSession).WithLastName("addressee").WithPartyContactMechanism(addresseeContact).Build();

            var carbonCopeeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("carbonCopee@allors.com").Build();
            var carbonCopeeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(carbonCopeeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var carbonCopee = new PersonBuilder(this.DatabaseSession).WithLastName("carbon copee").WithPartyContactMechanism(carbonCopeeContact).Build();

            var blindCopeeEmail = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("blindCopee@allors.com").Build();
            var blindCopeeContact = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(blindCopeeEmail).WithContactPurpose(personalEmailAddress).WithUseAsDefault(true).Build();
            var blindCopee = new PersonBuilder(this.DatabaseSession).WithLastName("blind copee").WithPartyContactMechanism(blindCopeeContact).Build();

            this.DatabaseSession.Derive(true);
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

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(5, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(originator, communication.InvolvedParties);
            Assert.Contains(addressee, communication.InvolvedParties);
            Assert.Contains(carbonCopee, communication.InvolvedParties);
            Assert.Contains(blindCopee, communication.InvolvedParties);
        }
    }
}