// <copyright file="AgreementItemTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class AgreementItemTest : DomainTest
    {
        [Fact]
        public void GivenAgreementExhibit_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AgreementExhibitBuilder(this.Session);
            var agreementExhibit = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("AgreementExhibit");
            agreementExhibit = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenAgreementPricingProgram_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AgreementPricingProgramBuilder(this.Session);
            var agreementPricingProgram = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("AgreementPricingProgram");
            agreementPricingProgram = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenAgreementSection_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new AgreementSectionBuilder(this.Session);
            var agreementSection = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("AgreementSection");
            agreementSection = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSubAgreement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new SubAgreementBuilder(this.Session);
            var subAgreement = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("SubAgreement");
            subAgreement = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
