// <copyright file="EstimatedProductCostTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class EstimatedProductCostTests : DomainTest
    {
        [Fact]
        public void GivenEstimatedLaborCost_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EstimatedLaborCostBuilder(this.Session);
            var laborCost = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCost(1);
            laborCost = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"));
            laborCost = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenEstimatedMaterialCost_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EstimatedMaterialCostBuilder(this.Session);
            var materialCost = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCost(1);
            materialCost = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"));
            materialCost = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenEstimatedOtherCost_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EstimatedOtherCostBuilder(this.Session);
            var otherCost = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCost(1);
            otherCost = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"));
            otherCost = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
