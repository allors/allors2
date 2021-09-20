// <copyright file="VatCalculationMethods.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class VatCalculationMethods
    {
        private static readonly Guid CashId = new Guid("0EBDC40A-B744-4518-8DEB-F060DEDB0FE6");
        private static readonly Guid InvoiceId = new Guid("180E1FEA-929E-46F4-9F5E-16E1DF60065F");

        private UniquelyIdentifiableSticky<VatCalculationMethod> cache;

        public VatCalculationMethod Cash => this.Cache[CashId];

        public VatCalculationMethod Invoice => this.Cache[InvoiceId];

        private UniquelyIdentifiableSticky<VatCalculationMethod> Cache => this.cache ??= new UniquelyIdentifiableSticky<VatCalculationMethod>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(CashId, v =>
            {
                v.Name = "Cash management scheme";
                localisedName.Set(v, dutchLocale, "Kasstelsel");
                v.IsActive = true;
            });

            merge(InvoiceId, v =>
            {
                v.Name = "Invoice management scheme";
                localisedName.Set(v, dutchLocale, "Factuurstelsel");
                v.IsActive = true;
            });
        }
    }
}
