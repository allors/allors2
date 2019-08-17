// <copyright file="AgreementTermTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class AgreementTermTest : DomainTest
    {
        [Fact]
        public void GivenFinancialTerm_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new FinancialTermBuilder(this.Session);
            var financialTerm = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("FinancialTerm");
            financialTerm = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithTermType(new OrderTermTypes(this.Session).NonReturnableSalesItem);
            financialTerm = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            financialTerm.RemoveDescription();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenIncentive_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new IncentiveBuilder(this.Session);
            var incentive = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("Incentive");
            incentive = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithTermType(new OrderTermTypes(this.Session).NonReturnableSalesItem);
            incentive = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            incentive.RemoveDescription();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenLegalTerm_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new LegalTermBuilder(this.Session);
            var legalTerm = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("LegalTerm");
            legalTerm = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithTermType(new OrderTermTypes(this.Session).NonReturnableSalesItem);
            legalTerm = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            legalTerm.RemoveDescription();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenThreshold_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ThresholdBuilder(this.Session);
            var threshold = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("Threshold");
            threshold = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithTermType(new OrderTermTypes(this.Session).NonReturnableSalesItem);
            threshold = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            threshold.RemoveDescription();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
