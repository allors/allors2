// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializedInventoryItemObjectStates.cs" company="Allors bvba">
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

    public partial class SerializedInventoryItemObjectStates
    {
        private static readonly Guid GoodId = new Guid("CD80FC9B-BF25-4587-8D52-57E491E74104");
        private static readonly Guid BeingReparedId = new Guid("4584A95F-60BA-478c-8E09-E6AFC88CF683");
        private static readonly Guid SlightlyDamagedId = new Guid("9CA506D4-ACB1-40ac-BEEC-83F080E6029E");
        private static readonly Guid DefectiveId = new Guid("36F94BB5-D93E-44cc-8F10-37A046002E5B");
        private static readonly Guid ScrapId = new Guid("9D02749B-A30E-4bb4-B016-E1CF96A5F99B");

        private UniquelyIdentifiableCache<SerializedInventoryItemObjectState> stateCache;

        public SerializedInventoryItemObjectState Good => this.StateCache.Get(GoodId);

        public SerializedInventoryItemObjectState BeingRepared => this.StateCache.Get(BeingReparedId);

        public SerializedInventoryItemObjectState SlightlyDamaged => this.StateCache.Get(SlightlyDamagedId);

        public SerializedInventoryItemObjectState Defective => this.StateCache.Get(DefectiveId);

        public SerializedInventoryItemObjectState Scrap => this.StateCache.Get(ScrapId);

        private UniquelyIdentifiableCache<SerializedInventoryItemObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<SerializedInventoryItemObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SerializedInventoryItemObjectStateBuilder(this.Session)
                .WithUniqueId(GoodId)
                .WithName("Good")
                .Build();

            new SerializedInventoryItemObjectStateBuilder(this.Session)
                .WithUniqueId(BeingReparedId)
                .WithName("Being Repared")
                .Build();

            new SerializedInventoryItemObjectStateBuilder(this.Session)
                .WithUniqueId(SlightlyDamagedId)
                .WithName("Slightly Damaged")
                .Build();

            new SerializedInventoryItemObjectStateBuilder(this.Session)
                .WithUniqueId(DefectiveId)
                .WithName("Defective")
                .Build();

            new SerializedInventoryItemObjectStateBuilder(this.Session)
                .WithUniqueId(ScrapId)
                .WithName("Scrap")
                .Build();
        }
    }
}