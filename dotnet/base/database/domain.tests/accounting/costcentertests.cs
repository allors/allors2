// <copyright file="CostCenterTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class CostCenterTests : DomainTest
    {
        [Fact]
        public void GivenCostCenter_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new CostCenterBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("CostCenter");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCostCenter_WhenDeriving_ThenPostBuildRelationsMustExist()
        {
            var costCenter = new CostCenterBuilder(this.Session)
                .WithName("CostCenter")
                .Build();

            Assert.True(costCenter.ExistUniqueId);
        }
    }
}
