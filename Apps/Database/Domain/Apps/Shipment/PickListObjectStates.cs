// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PickListObjectStates.cs" company="Allors bvba">
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

    public partial class PickListObjectStates
    {
        private static readonly Guid CreatedId = new Guid("E65872C0-AD3C-4802-A253-CF99F8209011");
        private static readonly Guid PickedId = new Guid("93C6B430-91BD-4e6c-AC2C-C287F2A8021D");
        private static readonly Guid CancelledId = new Guid("CD552AF5-E695-4329-BF87-5644C2EA98F3");
        private static readonly Guid OnHoldId = new Guid("1733E2B0-48CA-4731-8F3C-93C6CF3A9543");

        private UniquelyIdentifiableCache<PickListObjectState> stateCache;

        public PickListObjectState Created => this.StateCache.Get(CreatedId);

        public PickListObjectState Picked => this.StateCache.Get(PickedId);

        public PickListObjectState Cancelled => this.StateCache.Get(CancelledId);

        public PickListObjectState OnHold => this.StateCache.Get(OnHoldId);

        private UniquelyIdentifiableCache<PickListObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PickListObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englischLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PickListObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PickListObjectStateBuilder(this.Session)
                .WithUniqueId(PickedId)
                .WithName("Picked")
                .Build();

            new PickListObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PickListObjectStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();
        }
    }
}