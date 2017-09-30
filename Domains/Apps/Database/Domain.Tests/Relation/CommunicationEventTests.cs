//------------------------------------------------------------------------------------------------- 
// <copyright file="CommunicationEventTests.cs" company="Allors bvba">
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

    
    public class CommunicationEventTests : DomainTest
    {
        [Fact]
        public void GivenCommunicationEvent_WhenInProgress_ThenCurrentObjectStateIsInProgress()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new CommunicationEventStates(this.DatabaseSession).InProgress, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunicationEvent_WhenInPast_ThenCurrencObjectStateIsCompleted()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow.AddHours(-2))
                .WithActualEnd(DateTime.UtcNow.AddHours(-1))
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new CommunicationEventStates(this.DatabaseSession).Completed, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunicationEvent_WhenInFuture_ThenCurrencObjectStateIsScheduled()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow.AddHours(+1))
                .WithActualEnd(DateTime.UtcNow.AddHours(+2))
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new CommunicationEventStates(this.DatabaseSession).Scheduled, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenFaceToFaceCommunication_WhenConfirmed_ThenCurrentCommunicationEventStatusMustBeDerived()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.DatabaseSession)
                .WithOwner(new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithParticipant(new PersonBuilder(this.DatabaseSession).WithLastName("participant2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new CommunicationEventStates(this.DatabaseSession).InProgress, communication.CommunicationEventState);

            communication.Close();
            
            this.DatabaseSession.Derive();

            Assert.Equal(new CommunicationEventStates(this.DatabaseSession).Completed, communication.CommunicationEventState);
        }
    }
}