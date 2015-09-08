// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonSerializedInventoryItemObjectStates.cs" company="Allors bvba">
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

    public partial class NonSerializedInventoryItemObjectStates
    {
        public static readonly Guid GoodId = new Guid("6806CC54-3AA7-4510-A209-99F92D5C1D58");
        public static readonly Guid BeingReparedId = new Guid("ABD19809-9E27-4ee6-BEA1-637902260F57");
        public static readonly Guid SlightlyDamagedId = new Guid("EE4C6034-7320-4209-A0AA-6067B20AC418");
        public static readonly Guid DefectiveId = new Guid("C0E10011-1BA4-412f-B426-103C1C11B879");
        public static readonly Guid ScrapId = new Guid("CF51C221-111C-4666-8E97-CC060643C5FD");

        private UniquelyIdentifiableCache<NonSerializedInventoryItemObjectState> stateCache;

        public NonSerializedInventoryItemObjectState Good
        {
            get { return this.StateCache.Get(GoodId); }
        }

        public NonSerializedInventoryItemObjectState BeingRepared
        {
            get { return this.StateCache.Get(BeingReparedId); }
        }

        public NonSerializedInventoryItemObjectState SlightlyDamaged
        {
            get { return this.StateCache.Get(SlightlyDamagedId); }
        }

        public NonSerializedInventoryItemObjectState Defective
        {
            get { return this.StateCache.Get(DefectiveId); }
        }

        public NonSerializedInventoryItemObjectState Scrap
        {
            get { return this.StateCache.Get(ScrapId); }
        }

        private UniquelyIdentifiableCache<NonSerializedInventoryItemObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<NonSerializedInventoryItemObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new NonSerializedInventoryItemObjectStateBuilder(Session)
                .WithUniqueId(GoodId)
                .WithName("Good")
                .Build();

            new NonSerializedInventoryItemObjectStateBuilder(Session)
                .WithUniqueId(BeingReparedId)
                .WithName("Being Repared")
                .Build();

            new NonSerializedInventoryItemObjectStateBuilder(Session)
                .WithUniqueId(SlightlyDamagedId)
                .WithName("Slightly Damaged")
                .Build();

            new NonSerializedInventoryItemObjectStateBuilder(Session)
                .WithUniqueId(DefectiveId)
                .WithName("Defective")
                .Build();

            new NonSerializedInventoryItemObjectStateBuilder(Session)
                .WithUniqueId(ScrapId)
                .WithName("Scrap")
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