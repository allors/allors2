//------------------------------------------------------------------------------------------------- 
// <copyright file="WebSiteCommunicationTests.cs" company="Allors bvba">
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
    using Xunit;

    
    public class WebSiteCommunicationTests : DomainTest
    {
        [Fact]
        public void GivenWebSiteCommunication_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var person = new PersonBuilder(this.DatabaseSession).WithLastName("person").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();

            var builder = new WebSiteCommunicationBuilder(this.DatabaseSession).WithOriginator(person).WithReceiver(person);
            var communication = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            builder.WithSubject("Website communication");
            communication = builder.Build();

            this.DatabaseSession.Derive();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            Assert.Equal(communication.CommunicationEventState, new CommunicationEventStates(this.DatabaseSession).Scheduled);
            Assert.Equal(communication.CommunicationEventState, communication.LastCommunicationEventState);
        }

        [Fact]
        public void GivenWebSiteCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var originator = new PersonBuilder(this.DatabaseSession).WithLastName("originator").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var receiver = new PersonBuilder(this.DatabaseSession).WithLastName("receiver").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var communication = new WebSiteCommunicationBuilder(this.DatabaseSession)
                .WithSubject("Hello world!")
                .WithOwner(owner)
                .WithOriginator(originator)
                .WithReceiver(receiver)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(originator, communication.InvolvedParties);
            Assert.Contains(receiver, communication.InvolvedParties);
        }

        [Fact]
        public void GivenWebSiteCommunication_WhenOriginatorIsDeleted_ThenCommunicationEventIsDeleted()
        {
            var owner = new PersonBuilder(this.DatabaseSession).WithLastName("owner").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var originator = new PersonBuilder(this.DatabaseSession).WithLastName("originator").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var receiver = new PersonBuilder(this.DatabaseSession).WithLastName("receiver").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            new WebSiteCommunicationBuilder(this.DatabaseSession)
                .WithSubject("Hello world!")
                .WithOwner(owner)
                .WithOriginator(originator)
                .WithReceiver(receiver)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.DatabaseSession.Extent<WebSiteCommunication>().Count);

            originator.Delete();
            this.DatabaseSession.Derive();

            Assert.Equal(0, this.DatabaseSession.Extent<WebSiteCommunication>().Count);
        }
    }
}