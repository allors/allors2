// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceTermTypes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class InvoiceTermTypes
    {
        private static readonly Guid PaymentNetDaysId = new Guid("23AB7C88-C7B0-4a8e-916E-02DFD3CD261A");
        private static readonly Guid LateFeeId = new Guid("4D8E9C5E-F4F6-4e62-8009-D247D4C60753");
        private static readonly Guid CollectionAgencyPenaltyId = new Guid("56C719DE-1B5D-4c2a-8AE3-F205F9852C79");

        private UniquelyIdentifiableSticky<InvoiceTermType> cache;

        public InvoiceTermType PaymentNetDays => this.Cache[PaymentNetDaysId];

        public InvoiceTermType LateFee => this.Cache[LateFeeId];

        public InvoiceTermType CollectionAgencyPenalty => this.Cache[CollectionAgencyPenaltyId];
        private UniquelyIdentifiableSticky<InvoiceTermType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InvoiceTermType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var belgianLocale = new Locales(this.Session).DutchNetherlands;

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Payment-net days")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betaaltermijn").WithLocale(belgianLocale).Build())
                .WithUniqueId(PaymentNetDaysId)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Penalty for late fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete late betaling").WithLocale(belgianLocale).Build())
                .WithUniqueId(LateFeeId)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Penalty for collection agency")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete incassobureau").WithLocale(belgianLocale).Build())
                .WithUniqueId(CollectionAgencyPenaltyId)
                .Build();
        }
    }
}
