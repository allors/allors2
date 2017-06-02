//------------------------------------------------------------------------------------------------- 
// <copyright file="EstimatedProductCostTests.cs" company="Allors bvba">
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
    using System;
    using Meta;
    using Xunit;

    
    public class EstimatedProductCostTests : DomainTest
    {
        [Fact]
        public void GivenEstimatedLaborCost_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EstimatedLaborCostBuilder(this.DatabaseSession);
            var laborCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCost(1);
            laborCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"));
            laborCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            laborCost = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenEstimatedMaterialCost_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EstimatedMaterialCostBuilder(this.DatabaseSession);
            var materialCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCost(1);
            materialCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"));
            materialCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            materialCost = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenEstimatedOtherCost_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new EstimatedOtherCostBuilder(this.DatabaseSession);
            var otherCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCost(1);
            otherCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"));
            otherCost = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            otherCost = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}
