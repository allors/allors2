// <copyright file="EngagementRateTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class EngagementRateTests : DomainTest
    {
        [Fact]
        public void GivenEngagementRate_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EngagementRateBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBillingRate(10M);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithRateType(new RateTypes(this.Session).StandardRate);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
