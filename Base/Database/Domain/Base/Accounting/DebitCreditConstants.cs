// <copyright file="DebitCreditConstants.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class DebitCreditConstants
    {
        private static readonly Guid DebitId = new Guid("C957ED48-7A31-4308-8CC5-03C8014A8646");
        private static readonly Guid CreditId = new Guid("BECDF0E7-C2DD-4ddf-A1A0-FC5E9E15F0A8");

        private UniquelyIdentifiableSticky<DebitCreditConstant> cache;

        public DebitCreditConstant Debit => this.Cache[DebitId];

        public DebitCreditConstant Credit => this.Cache[CreditId];

        private UniquelyIdentifiableSticky<DebitCreditConstant> Cache => this.cache ??= new UniquelyIdentifiableSticky<DebitCreditConstant>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(DebitId, v =>
            {
                v.Name = "Debit";
                localisedName.Set(v, dutchLocale, "Debet");
                v.IsActive = true;
            });

            merge(CreditId, v =>
            {
                v.Name = "Credit";
                localisedName.Set(v, dutchLocale, "Credit");
                v.IsActive = true;
            });
        }
    }
}
