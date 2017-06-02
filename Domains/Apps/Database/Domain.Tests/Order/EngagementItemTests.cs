//------------------------------------------------------------------------------------------------- 
// <copyright file="EngagementItemTests.cs" company="Allors bvba">
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

    
    public class EngagementItemTests : DomainTest
    {
        [Fact]
        public void GivenCustomEngagementItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new CustomEngagementItemBuilder(this.DatabaseSession);
            var customEngagementItem = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("CustomEngagementItem");
            customEngagementItem = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenDeliverableOrderItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new DeliverableOrderItemBuilder(this.DatabaseSession);
            var deliverableOrderItem = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("DeliverableOrderItem");
            deliverableOrderItem = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenGoodOrderItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new GoodOrderItemBuilder(this.DatabaseSession);
            var goodOrderItem = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("GoodOrderItem");
            goodOrderItem = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProfessionalPlacement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ProfessionalPlacementBuilder(this.DatabaseSession);
            var professionalPlacement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("ProfessionalPlacement");
            professionalPlacement = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenStandardServiceOrderItem_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new StandardServiceOrderItemBuilder(this.DatabaseSession);
            var standardServiceOrderItem = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("StandardServiceOrderItem");
            standardServiceOrderItem = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}
