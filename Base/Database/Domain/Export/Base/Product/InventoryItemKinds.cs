// <copyright file="InventoryItemKinds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class InventoryItemKinds
    {
        private static readonly Guid SerialisedId = new Guid("2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE");
        private static readonly Guid NonSerialisedId = new Guid("EAA6C331-0DD9-4bb1-8245-12A673304468");

        private UniquelyIdentifiableSticky<InventoryItemKind> cache;

        public InventoryItemKind Serialised => this.Cache[SerialisedId];

        public InventoryItemKind NonSerialised => this.Cache[NonSerialisedId];

        private UniquelyIdentifiableSticky<InventoryItemKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InventoryItemKind>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InventoryItemKindBuilder(this.Session)
                .WithName("Serialised")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Op serienummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(SerialisedId)
                .WithIsActive(true)
                .Build();

            new InventoryItemKindBuilder(this.Session)
                .WithName("Non serialised")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zonder serienummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(NonSerialisedId)
                .WithIsActive(true)
                .Build();
        }
    }
}
