// <copyright file="FaceToFaceCommunicationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class FaceToFaceCommunicationTests : DomainTest
    {
        [Fact]
        public void GivenFaceToFaceCommunicationIsBuild_WhenDeriving_ThenStatusIsSet()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithSubject("subject")
                .WithFromParty(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithToParty(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithActualStart(this.Session.Now())
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            Assert.Equal(communication.CommunicationEventState, new CommunicationEventStates(this.Session).InProgress);
            Assert.Equal(communication.CommunicationEventState, communication.LastCommunicationEventState);
        }

        [Fact]
        public void GivenFaceToFaceCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.Session).WithLastName("participant2").Build();

            this.Session.Derive();
            this.Session.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithFromParty(participant1)
                .WithToParty(participant2)
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(participant2, communication.InvolvedParties);
            Assert.Contains(owner, communication.InvolvedParties);
        }

        [Fact]
        public void GivenFaceToFaceCommunication_WhenParticipantIsDeleted_ThenCommunicationEventIsDeleted()
        {
            var participant1 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.Session).WithLastName("participant2").Build();

            this.Session.Derive();
            this.Session.Commit();

            new FaceToFaceCommunicationBuilder(this.Session)
                .WithSubject("subject")
                .WithFromParty(participant1)
                .WithToParty(participant2)
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Single(this.Session.Extent<FaceToFaceCommunication>());

            participant2.Delete();
            this.Session.Derive();

            Assert.Equal(0, this.Session.Extent<FaceToFaceCommunication>().Count);
        }
    }

    [Trait("Category", "Security")]
    public class FaceToFaceCommunicationSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenCurrentUserIsUnknown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.Session).WithLastName("participant2").Build();

            this.Session.Derive();
            this.Session.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithFromParty(participant1)
                .WithToParty(participant2)
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(2, communication.SecurityTokens.Count);
            Assert.Contains(new SecurityTokens(this.Session).DefaultSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }

        [Fact]
        public void GivenCurrentUserIsKnown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.Session).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.Session).WithLastName("participant2").Build();

            this.Session.Derive();
            this.Session.Commit();

            this.Session.SetUser(owner);

            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithFromParty(participant1)
                .WithToParty(participant2)
                .WithActualStart(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(2, communication.SecurityTokens.Count);
            Assert.Contains(new SecurityTokens(this.Session).DefaultSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }
    }
}
