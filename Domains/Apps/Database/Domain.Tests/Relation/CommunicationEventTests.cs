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
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunicationEvent_WhenInPast_ThenCurrencObjectStateIsCompleted()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow.AddHours(-2))
                .WithActualEnd(DateTime.UtcNow.AddHours(-1))
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenCommunicationEvent_WhenInFuture_ThenCurrencObjectStateIsScheduled()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow.AddHours(+1))
                .WithActualEnd(DateTime.UtcNow.AddHours(+2))
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).Scheduled, communication.CommunicationEventState);
        }

        [Fact]
        public void GivenFaceToFaceCommunication_WhenConfirmed_ThenCurrentCommunicationEventStatusMustBeDerived()
        {
            var communication = new FaceToFaceCommunicationBuilder(this.Session)
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant1").Build())
                .WithParticipant(new PersonBuilder(this.Session).WithLastName("participant2").Build())
                .WithSubject("Hello")
                .WithActualStart(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, communication.CommunicationEventState);

            communication.Close();
            
            this.Session.Derive();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communication.CommunicationEventState);
        }
    }
}