
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

        private UniquelyIdentifiableSticky<DebitCreditConstant> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<DebitCreditConstant>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Debit")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Debet").WithLocale(dutchLocale).Build())
                .WithUniqueId(DebitId)
                .WithIsActive(true)
                .Build();

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Credit")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Credit").WithLocale(dutchLocale).Build())
                .WithUniqueId(CreditId)
                .WithIsActive(true)
                .Build();
        }
    }
}
