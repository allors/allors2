// <copyright file="JournalTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class JournalTypes
    {
        private static readonly Guid CashId = new Guid("B057AB5B-3BFD-46A4-80AA-D5330252E7E8");
        private static readonly Guid BankId = new Guid("D09AAFCB-CD28-447F-BE28-92509B7723CB");
        private static readonly Guid GiroId = new Guid("D364CC2B-9173-454F-A81E-F012859DAE44");
        private static readonly Guid GeneralId = new Guid("B886672A-019A-4D27-BFA0-6C18A8BBD4B8");
        private static readonly Guid SalesId = new Guid("FAE9D5DB-0EAD-46CB-89E8-C32E4001D8D1");
        private static readonly Guid PurchaseId = new Guid("BB8A39C3-9F3E-4D3F-83A2-10100016A78E");

        private UniquelyIdentifiableSticky<JournalType> cache;

        public JournalType Cash => this.Cache[CashId];

        public JournalType Bank => this.Cache[BankId];

        public JournalType Giro => this.Cache[GiroId];

        public JournalType General => this.Cache[GeneralId];

        public JournalType Sales => this.Cache[SalesId];

        public JournalType Purchase => this.Cache[PurchaseId];

        private UniquelyIdentifiableSticky<JournalType> Cache => this.cache ??= new UniquelyIdentifiableSticky<JournalType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(CashId, v =>
            {
                v.Name = "Cash";
                localisedName.Set(v, dutchLocale, "Kas");
                v.IsActive = true;
            });

            merge(BankId, v =>
            {
                v.Name = "Bank";
                localisedName.Set(v, dutchLocale, "Bank");
                v.IsActive = true;
            });

            merge(GiroId, v =>
            {
                v.Name = "Giro";
                localisedName.Set(v, dutchLocale, "Giro");
                v.IsActive = true;
            });

            merge(GeneralId, v =>
            {
                v.Name = "General";
                localisedName.Set(v, dutchLocale, "Memoriaal");
                v.IsActive = true;
            });

            merge(SalesId, v =>
            {
                v.Name = "Sales";
                localisedName.Set(v, dutchLocale, "Verkoop");
                v.IsActive = true;
            });

            merge(PurchaseId, v =>
            {
                v.Name = "Purchase";
                localisedName.Set(v, dutchLocale, "Aankoop");
                v.IsActive = true;
            });
        }
    }
}
