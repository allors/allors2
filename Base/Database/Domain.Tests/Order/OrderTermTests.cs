// <copyright file="OrderTermTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class OrderTermTests : DomainTest
    {
        [Fact]
        public void GivenOrderTerm_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new OrderTermBuilder(this.Session);
            var salesTerm = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithTermType(new OrderTermTypes(this.Session).PercentageCancellationCharge);
            salesTerm = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
