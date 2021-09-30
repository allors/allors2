// <copyright file="FaxCommunicationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class FaxCommunicationTests : DomainTest
    {
        [Fact]
        public void GivenFaxCommunicationIsBuild_WhenDeriving_ThenStatusIsSet()
        {
            var communication = new FaxCommunicationBuilder(this.Session)
                .WithSubject("subject")
                .WithOwner(new PersonBuilder(this.Session).WithLastName("owner").Build())
                .WithFromParty(new PersonBuilder(this.Session).WithLastName("originator").Build())
                .WithToParty(new PersonBuilder(this.Session).WithLastName("receiver").Build())
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            Assert.Equal(communication.CommunicationEventState, new CommunicationEventStates(this.Session).Scheduled);
            Assert.Equal(communication.CommunicationEventState, communication.LastCommunicationEventState);
        }

        [Fact]
        public void GivenFaxCommunication_WhenDeriving_ThenInvolvedPartiesAreDerived()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var originator = new PersonBuilder(this.Session).WithLastName("originator").Build();
            var receiver = new PersonBuilder(this.Session).WithLastName("receiver").Build();

            this.Session.Derive();
            this.Session.Commit();

            var communication = new FaxCommunicationBuilder(this.Session)
                .WithSubject("subject")
                .WithOwner(owner)
                .WithFromParty(originator)
                .WithToParty(receiver)
                .Build();

            this.Session.Derive();

            Assert.Equal(3, communication.InvolvedParties.Count);
            Assert.Contains(owner, communication.InvolvedParties);
            Assert.Contains(originator, communication.InvolvedParties);
            Assert.Contains(receiver, communication.InvolvedParties);
        }

        [Fact]
        public void GivenFaxCommunication_WhenOriginatorIsDeleted_ThenCommunicationEventIsDeleted()
        {
            var owner = new PersonBuilder(this.Session).WithLastName("owner").Build();
            var originator = new PersonBuilder(this.Session).WithLastName("originator").Build();
            var receiver = new PersonBuilder(this.Session).WithLastName("receiver").Build();

            this.Session.Derive();
            this.Session.Commit();

            new FaxCommunicationBuilder(this.Session)
                .WithSubject("Hello world!")
                .WithOwner(owner)
                .WithFromParty(originator)
                .WithToParty(receiver)
                .Build();

            this.Session.Derive();

            Assert.Single(this.Session.Extent<FaxCommunication>());

            originator.Delete();
            this.Session.Derive();

            Assert.Equal(0, this.Session.Extent<FaxCommunication>().Count);
        }
    }
}
