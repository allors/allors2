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
        private static readonly Guid PaymentNetDaysId = new Guid("23AB7C88-C7B0-4A8E-916E-02DFD3CD261A");
        private static readonly Guid LateFeeId = new Guid("4D8E9C5E-F4F6-4e62-8009-D247D4C60753");
        private static readonly Guid CollectionAgencyPenaltyId = new Guid("56C719DE-1B5D-4c2a-8AE3-F205F9852C79");
        private static readonly Guid PayFullBeforeTransportId = new Guid("B4C38969-9507-4D44-A644-CF8CA6ADB79A");
        private static readonly Guid PayHalfBeforeTransportId = new Guid("F5B1107F-A325-4E97-94AE-ACEFC306C465");
        private static readonly Guid OtherId = new Guid("9A8F6C1F-B590-408F-B42E-0AF12F42C14B");

        private UniquelyIdentifiableSticky<InvoiceTermType> cache;

        public InvoiceTermType PaymentNetDays => this.Cache[PaymentNetDaysId];

        public InvoiceTermType LateFee => this.Cache[LateFeeId];

        public InvoiceTermType CollectionAgencyPenalty => this.Cache[CollectionAgencyPenaltyId];

        public InvoiceTermType PayFullBeforeTransport => this.Cache[PayFullBeforeTransportId];

        public InvoiceTermType PayHalfBeforeTransport => this.Cache[PayHalfBeforeTransportId];

        public InvoiceTermType Other => this.Cache[OtherId];

        private UniquelyIdentifiableSticky<InvoiceTermType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InvoiceTermType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var belgianLocale = new Locales(this.Session).DutchNetherlands;

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Payment-net days")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betaaltermijn").WithLocale(belgianLocale).Build())
                .WithUniqueId(PaymentNetDaysId)
                .WithIsActive(true)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Penalty for late fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete late betaling").WithLocale(belgianLocale).Build())
                .WithUniqueId(LateFeeId)
                .WithIsActive(true)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Penalty for collection agency")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete incassobureau").WithLocale(belgianLocale).Build())
                .WithUniqueId(CollectionAgencyPenaltyId)
                .WithIsActive(true)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Payment condition: 100% before transport")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betalings voorwaarde: 100% voorafgaand aan verzending").WithLocale(belgianLocale).Build())
                .WithUniqueId(PayFullBeforeTransportId)
                .WithIsActive(true)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Payment condition: 50% on order and 50% before transport")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betalings voorwaarde: 50% bij order en 50% voorafgaand aan verzending").WithLocale(belgianLocale).Build())
                .WithUniqueId(PayHalfBeforeTransportId)
                .WithIsActive(true)
                .Build();

            new InvoiceTermTypeBuilder(this.Session)
                .WithName("Other")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overige").WithLocale(belgianLocale).Build())
                .WithUniqueId(PayHalfBeforeTransportId)
                .WithIsActive(true)
                .Build();
        }
    }
}
