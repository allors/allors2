// <copyright file="ChartOfAccountsTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class ChartOfAccountsTests : DomainTest
    {
        [Fact]
        public void GivenChartOfAccounts_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ChartOfAccountsBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("ChartOfAccounts");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenChartOfAccounts_WhenDeriving_ThenPostBuildRelationsMustExist()
        {
            var chartOfAccounts = new ChartOfAccountsBuilder(this.Session)
                .WithName("ChartOfAccounts")
                .Build();

            Assert.True(chartOfAccounts.ExistUniqueId);
        }
    }
}
