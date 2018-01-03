//------------------------------------------------------------------------------------------------- 
// <copyright file="AgreementTermTest.cs" company="Allors bvba">
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

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithTermType(new SalesTermTypes(this.Session).NonReturnableSalesItem);
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

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithTermType(new SalesTermTypes(this.Session).NonReturnableSalesItem);
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

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithTermType(new SalesTermTypes(this.Session).NonReturnableSalesItem);
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

            Assert.False(this.Session.Derive(false).HasErrors);
            builder.WithTermType(new SalesTermTypes(this.Session).NonReturnableSalesItem);
            threshold = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            threshold.RemoveDescription();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
