// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemKinds.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class InventoryItemKinds
    {
        public static readonly Guid SerializedId = new Guid("2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE");
        public static readonly Guid NonSerializedId = new Guid("EAA6C331-0DD9-4bb1-8245-12A673304468");

        private UniquelyIdentifiableCache<InventoryItemKind> cache;

        public InventoryItemKind Serialized
        {
            get { return this.Cache.Get(SerializedId); }
        }

        public InventoryItemKind NonSerialized
        {
            get { return this.Cache.Get(NonSerializedId); }
        }

        private UniquelyIdentifiableCache<InventoryItemKind> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<InventoryItemKind>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InventoryItemKindBuilder(this.Session)
                .WithName("Serialized")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Serialized").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Op serienummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(SerializedId)
                .Build();
            
            new InventoryItemKindBuilder(this.Session)
                .WithName("Non serialized")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Non serialized").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zonder serienummer").WithLocale(dutchLocale).Build())
                .WithUniqueId(NonSerializedId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
