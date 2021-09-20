// <copyright file="InvoiceTermTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class InvoiceTermTypes
    {
        public static readonly Guid PaymentNetDaysId = new Guid("23AB7C88-C7B0-4A8E-916E-02DFD3CD261A");
        public static readonly Guid LateFeeId = new Guid("4D8E9C5E-F4F6-4e62-8009-D247D4C60753");
        public static readonly Guid CollectionAgencyPenaltyId = new Guid("56C719DE-1B5D-4c2a-8AE3-F205F9852C79");
        public static readonly Guid PayFullBeforeTransportId = new Guid("B4C38969-9507-4D44-A644-CF8CA6ADB79A");
        public static readonly Guid PayHalfBeforeTransportId = new Guid("F5B1107F-A325-4E97-94AE-ACEFC306C465");
        public static readonly Guid OtherId = new Guid("9A8F6C1F-B590-408F-B42E-0AF12F42C14B");

        private UniquelyIdentifiableSticky<InvoiceTermType> cache;

        public InvoiceTermType PaymentNetDays => this.Cache[PaymentNetDaysId];

        public InvoiceTermType LateFee => this.Cache[LateFeeId];

        public InvoiceTermType CollectionAgencyPenalty => this.Cache[CollectionAgencyPenaltyId];

        public InvoiceTermType PayFullBeforeTransport => this.Cache[PayFullBeforeTransportId];

        public InvoiceTermType PayHalfBeforeTransport => this.Cache[PayHalfBeforeTransportId];

        public InvoiceTermType Other => this.Cache[OtherId];

        private UniquelyIdentifiableSticky<InvoiceTermType> Cache => this.cache ??= new UniquelyIdentifiableSticky<InvoiceTermType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(PaymentNetDaysId, v =>
            {
                v.Name = "Payment-net days";
                localisedName.Set(v, dutchLocale, "Betaaltermijn");
                v.IsActive = true;
            });

            merge(LateFeeId, v =>
            {
                v.Name = "Penalty for late fee";
                localisedName.Set(v, dutchLocale, "Boete late betaling");
                v.IsActive = true;
            });

            merge(CollectionAgencyPenaltyId, v =>
            {
                v.Name = "Penalty for collection agency";
                localisedName.Set(v, dutchLocale, "Boete incassobureau");
                v.IsActive = true;
            });

            merge(PayFullBeforeTransportId, v =>
            {
                v.Name = "Payment condition: 100% before transport";
                localisedName.Set(v, dutchLocale, "Betalings voorwaarde: 100% voorafgaand aan verzending");
                v.IsActive = true;
            });

            merge(PayHalfBeforeTransportId, v =>
            {
                v.Name = "Payment condition: 50% on order and 50% before transport";
                localisedName.Set(v, dutchLocale, "Betalings voorwaarde: 50% bij order en 50% voorafgaand aan verzending");
                v.IsActive = true;
            });

            merge(OtherId, v =>
            {
                v.Name = "Other";
                localisedName.Set(v, dutchLocale, "Overige");
                v.IsActive = true;
            });
        }
    }
}
