// <copyright file="PurchaseInvoiceTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseInvoiceTypes
    {
        private static readonly Guid PurchaseInvoiceId = new Guid("D08F0309-A4CB-4ab7-8F75-3BB11DCF3783");
        private static readonly Guid PurchaseReturnId = new Guid("0187D927-81F5-4d6a-9847-58B674AD3E6A");

        private UniquelyIdentifiableSticky<PurchaseInvoiceType> cache;

        public PurchaseInvoiceType PurchaseInvoice => this.Cache[PurchaseInvoiceId];

        public PurchaseInvoiceType PurchaseReturn => this.Cache[PurchaseReturnId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceType> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseInvoiceType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(PurchaseInvoiceId, v =>
            {
                v.Name = "Purchase invoice";
                localisedName.Set(v, dutchLocale, "Aankoop factuur");
                v.IsActive = true;
            });

            merge(PurchaseReturnId, v =>
            {
                v.Name = "Purchase return";
                localisedName.Set(v, dutchLocale, "Aankoop factuur retour");
                v.IsActive = true;
            });
        }
    }
}
