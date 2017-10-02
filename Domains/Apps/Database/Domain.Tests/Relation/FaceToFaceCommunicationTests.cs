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
            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build())
                .WithSubject("subject")
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build())
                .WithActualStart(DateTime.UtcNow)
                .Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            Assert.Equal(communication.CommunicationEventState, new CommunicationEventStates(this.DatabaseSession).InProgress);
            Assert.Equal(communication.CommunicationEventState, communication.LastCommunicationEventState);
        }

        [Fact]
        public void GivenFaceToFaceCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();
            
            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(participant2, communication.InvolvedParties);
            Assert.Contains(owner, communication.InvolvedParties);

            communication.AddParticipant(owner);

            this.DatabaseSession.Derive();
            
            Assert.Equal(3, communication.InvolvedParties.Count);
        }

        [Fact]
        public void GivenCurrentUserIsUnknown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(2, communication.SecurityTokens.Count);
            Assert.Contains(Singleton.Instance(this.DatabaseSession).DefaultSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }

        [Fact]
        public void GivenCurrentUserIsKnown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            this.SetIdentity("user");

            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(2, communication.SecurityTokens.Count);
            Assert.Contains(Singleton.Instance(this.DatabaseSession).DefaultSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }


        [Fact]
        public void GivenFaceToFaceCommunication_WhenParticipantIsDeleted_ThenCommunicationEventIsDeleted()
        {
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.DatabaseSession.Extent<FaceToFaceCommunication>().Count);

            participant2.Delete();
            this.DatabaseSession.Derive();

            Assert.Equal(0, this.DatabaseSession.Extent<FaceToFaceCommunication>().Count);
        }

    }
}