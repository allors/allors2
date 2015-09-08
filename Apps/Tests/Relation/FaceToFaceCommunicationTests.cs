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
    using System.Security.Principal;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class FaceToFaceCommunicationTests : DomainTest
    {
        [Test]
        public void GivenFaceToFaceCommunication_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new FaceToFaceCommunicationBuilder(this.DatabaseSession);
            var communication = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSubject("subject");
            communication = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant1").Build());
            communication = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            Assert.AreEqual(communication.CurrentCommunicationEventStatus.CommunicationEventObjectState, new CommunicationEventObjectStates(this.DatabaseSession).Scheduled);
            Assert.AreEqual(communication.CurrentObjectState, new CommunicationEventObjectStates(this.DatabaseSession).Scheduled);
            Assert.AreEqual(communication.CurrentObjectState, communication.LastObjectState);
        }

        [Test]
        public void GivenFaceToFaceCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            
            Assert.AreEqual(3, communication.InvolvedParties.Count);
            Assert.Contains(participant1, communication.InvolvedParties);
            Assert.Contains(participant2, communication.InvolvedParties);
            Assert.Contains(owner, communication.InvolvedParties);

            communication.AddParticipant(owner);

            this.DatabaseSession.Derive(true);
            
            Assert.AreEqual(3, communication.InvolvedParties.Count);
        }

        [Test]
        public void GivenCurrentUserIsUnknown_WhenAccessingFaceToFaceCommunicationWithoutOwner_ThenAdminSecurityTokenIsApplied()
        {
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, communication.SecurityTokens.Count);
            Assert.Contains(Singleton.Instance(this.DatabaseSession).AdministratorSecurityToken, communication.SecurityTokens);
        }

        [Test]
        public void GivenCurrentUserIsUnknown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, communication.SecurityTokens.Count);
            Assert.Contains(Singleton.Instance(this.DatabaseSession).AdministratorSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }

        [Test]
        public void GivenCurrentUserIsKnown_WhenAccessingFaceToFaceCommunicationWithOwner_ThenOwnerSecurityTokenIsApplied()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("user"), new string[0]);

            var user = new PersonBuilder(this.DatabaseSession).WithLastName("user").WithUserName("user").Build();

            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").Build();
            var participant1 = new PersonBuilder(this.DatabaseSession).WithLastName("participant1").Build();
            var participant2 = new PersonBuilder(this.DatabaseSession).WithLastName("participant2").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(owner)
                .WithSubject("subject")
                .WithParticipant(participant1)
                .WithParticipant(participant2)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, communication.SecurityTokens.Count);
            Assert.Contains(Singleton.Instance(this.DatabaseSession).AdministratorSecurityToken, communication.SecurityTokens);
            Assert.Contains(owner.OwnerSecurityToken, communication.SecurityTokens);
        }
    }
}