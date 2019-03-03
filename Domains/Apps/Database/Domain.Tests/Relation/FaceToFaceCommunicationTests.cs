//------------------------------------------------------------------------------------------------- 
// <copyright file="FaceToFaceCommunicationTests.cs" company="Allors bvba">
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
    using System;
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
                .WithActualStart(DateTime.UtcNow)
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
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.Session.Derive();
            
            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(participant2, communication.InvolvedParties);
            Assert.Contains(owner, communication.InvolvedParties);
        }

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
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(2, communication.SecurityTokens.Count);
            Assert.Contains(this.Session.GetSingleton().DefaultSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }

        [Fact]
        public void GivenCurrentUserIsKnown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            this.SetIdentity("user");

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
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(2, communication.SecurityTokens.Count);
            Assert.Contains(this.Session.GetSingleton().DefaultSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
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
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Single(this.Session.Extent<FaceToFaceCommunication>());

            participant2.Delete();
            this.Session.Derive();

            Assert.Equal(0, this.Session.Extent<FaceToFaceCommunication>().Count);
        }

    }
}