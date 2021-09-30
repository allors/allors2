// <copyright file="EngagementItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class EngagementItemTests : DomainTest
    {
        [Fact]
        public void GivenCustomEngagementItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new CustomEngagementItemBuilder(this.Session);
            var customEngagementItem = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("CustomEngagementItem");
            customEngagementItem = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenDeliverableOrderItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new DeliverableOrderItemBuilder(this.Session);
            var deliverableOrderItem = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("DeliverableOrderItem");
            deliverableOrderItem = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenGoodOrderItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new GoodOrderItemBuilder(this.Session);
            var goodOrderItem = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("GoodOrderItem");
            goodOrderItem = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProfessionalPlacement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ProfessionalPlacementBuilder(this.Session);
            var professionalPlacement = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("ProfessionalPlacement");
            professionalPlacement = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenStandardServiceOrderItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new StandardServiceOrderItemBuilder(this.Session);
            var standardServiceOrderItem = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("StandardServiceOrderItem");
            standardServiceOrderItem = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
