// <copyright file="CostCenterSplitMethods.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CostCenterSplitMethods
    {
        private static readonly Guid Topid = new Guid("844444D0-5D69-4A3B-9CB6-0D98747839D8");
        private static readonly Guid BottomId = new Guid("13AE2935-F926-426F-BB1C-979BE7F3DF0B");

        private UniquelyIdentifiableSticky<CostCenterSplitMethod> cache;

        public CostCenterSplitMethod Top => this.Cache[Topid];

        public CostCenterSplitMethod Bottom => this.Cache[BottomId];

        private UniquelyIdentifiableSticky<CostCenterSplitMethod> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<CostCenterSplitMethod>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Use top level´s cost center GL-account")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gebruik grootboekrekening van kostenplaats van hoogste niveau").WithLocale(dutchLocale).Build())
                .WithUniqueId(Topid)
                .WithIsActive(true)
                .Build();

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Use bottom level´s cost center GL-account")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gebruik grootboekrekening van kostenplaats van laagste niveau").WithLocale(dutchLocale).Build())
                .WithUniqueId(BottomId)
                .WithIsActive(true)
                .Build();
        }
    }
}
