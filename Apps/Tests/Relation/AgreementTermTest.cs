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
    

    
    using NUnit.Framework;

    [TestFixture]
    public class AgreementTermTest : DomainTest
    {
        [Test]
        public void GivenFinancialTerm_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new FinancialTermBuilder(this.DatabaseSession);
            var financialTerm = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("FinancialTerm");
            financialTerm = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithTermType(new TermTypes(this.DatabaseSession).NonReturnableSalesItem);
            financialTerm = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            financialTerm.RemoveDescription();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenIncentive_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new IncentiveBuilder(this.DatabaseSession);
            var incentive = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();
            
            builder.WithDescription("Incentive");
            incentive = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithTermType(new TermTypes(this.DatabaseSession).NonReturnableSalesItem);
            incentive = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            incentive.RemoveDescription();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenLegalTerm_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new LegalTermBuilder(this.DatabaseSession);
            var legalTerm = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("LegalTerm");
            legalTerm = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithTermType(new TermTypes(this.DatabaseSession).NonReturnableSalesItem);
            legalTerm = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            legalTerm.RemoveDescription();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenThreshold_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ThresholdBuilder(this.DatabaseSession);
            var threshold = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("Threshold");
            threshold = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
            builder.WithTermType(new TermTypes(this.DatabaseSession).NonReturnableSalesItem);
            threshold = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            threshold.RemoveDescription();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
