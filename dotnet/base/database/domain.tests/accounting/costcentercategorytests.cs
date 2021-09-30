// <copyright file="CostCenterCategoryTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class CostCenterCategoryTests : DomainTest
    {
        [Fact]
        public void GivenCostCenterCategory_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new CostCenterCategoryBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("CostCenterCategory");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCostCenterCategory_WhenDeriving_ThenPostBuildRelationsMustExist()
        {
            var costCenterCategory = new CostCenterCategoryBuilder(this.Session)
                .WithDescription("CostCenterCategory")
                .Build();

            Assert.True(costCenterCategory.ExistUniqueId);
        }
    }
}
