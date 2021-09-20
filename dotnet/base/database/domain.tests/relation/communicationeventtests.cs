// <copyright file="CommunicationEventTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class CommunicationEventTests : DomainTest
    {
        [Fact]
        public void GivenCommunicationEvent_WhenInProgress_ThenCurrentObjectStateIsInProgress()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithFromParty(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithToParty(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunicationEvent_WhenInPast_ThenCurrencObjectStateIsCompleted()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithFromParty(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithToParty(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now().AddHours(-2))
                .WithActualEnd(this.Session.Now().AddHours(-1))
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunicationEvent_WhenInFuture_ThenCurrencObjectStateIsScheduled()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithFromParty(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithToParty(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now().AddHours(+1))
                .WithActualEnd(this.Session.Now().AddHours(+2))
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).Scheduled, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenFaceToFaceCommunication_WhenConfirmed_ThenCurrentCommunicationEventStatusMustBeDerived()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithFromParty(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithToParty(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, communication.CommunicationEventState);

            communication.Close();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunication_WhenDerived_ThenCommunicationEventIsAddedToEachParty()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.Session).WithLastName("participant2").Build();

            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(owner)
                .WithFromParty(participant1)
                .WithToParty(participant2)
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(participant2, communication.InvolvedParties);
        }

        [Fact]
        public void GivenCommunicationToOrganisation_WhenDerived_ThenCommunicationEventIsAddedToEachParty()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var participant = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").Build();
            var contact = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            new OrganisationContactRelationshipBuilder(this.Session).WithContact(contact).WithOrganisation(organisation).Build();

            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(owner)
                .WithFromParty(participant)
                .WithToParty(contact)
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(4, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(participant, communication.InvolvedParties);
            Assert.Contains(contact, communication.InvolvedParties);
            Assert.Contains(organisation, communication.InvolvedParties);
        }

        [Fact]
        public void GivenCommunication_WhenPartiesChange_ThenPartiesAreUpdated()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").Build();
            var contact = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            new OrganisationContactRelationshipBuilder(this.Session).WithContact(contact).WithOrganisation(organisation).Build();

            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(owner)
                .WithFromParty(participant1)
                .WithToParty(contact)
                .WithSubject("Hello")
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(4, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(contact, communication.InvolvedParties);
            Assert.Contains(organisation, communication.InvolvedParties);

            communication.ToParty = participant2;

            this.Session.Derive();

            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(participant2, communication.InvolvedParties);
        }
    }
}
