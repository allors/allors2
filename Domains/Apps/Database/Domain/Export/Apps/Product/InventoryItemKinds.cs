// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemKinds.cs" company="Allors bvba">
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

    public partial class InventoryItemKinds
    {
        private static readonly Guid SerialisedId = new Guid("2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE");
        private static readonly Guid NonSerialisedId = new Guid("EAA6C331-0DD9-4bb1-8245-12A673304468");

        private UniquelyIdentifiableSticky<InventoryItemKind> cache;

        public InventoryItemKind Serialised => this.Cache[SerialisedId];

        public InventoryItemKind NonSerialised => this.Cache[NonSerialisedId];

        private UniquelyIdentifiableSticky<InventoryItemKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InventoryItemKind>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InventoryItemKindBuilder(this.Session)
                .WithName("Serialised")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Op serienummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(SerialisedId)
                .Build();
            
            new InventoryItemKindBuilder(this.Session)
                .WithName("Non serialised")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zonder serienummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(NonSerialisedId)
                .Build();
        }
    }
}
