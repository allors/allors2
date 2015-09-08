//------------------------------------------------------------------------------------------------- 
// <copyright file="AgreementItemTest.cs" company="Allors bvba">
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
    public class AgreementItemTest : DomainTest
    {
        [Test]
        public void GivenAgreementExhibit_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AgreementExhibitBuilder(this.DatabaseSession);
            var agreementExhibit = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("AgreementExhibit");
            agreementExhibit = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenAgreementPricingProgram_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AgreementPricingProgramBuilder(this.DatabaseSession);
            var agreementPricingProgram = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("AgreementPricingProgram");
            agreementPricingProgram = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenAgreementSection_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AgreementSectionBuilder(this.DatabaseSession);
            var agreementSection = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("AgreementSection");
            agreementSection = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSubAgreement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new SubAgreementBuilder(this.DatabaseSession);
            var subAgreement = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("SubAgreement");
            subAgreement = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
