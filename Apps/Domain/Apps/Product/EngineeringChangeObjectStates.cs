// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringChangeObjectStates.cs" company="Allors bvba">
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

    public partial class EngineeringChangeObjectStates
    {
        public static readonly Guid RequestedId = new Guid("1732B578-2CA4-40b5-95B5-6B39D453CF87");
        public static readonly Guid NoticedId = new Guid("811E1661-B788-4c89-BE46-D5DD3B1EE20B");
        public static readonly Guid ReleasedId = new Guid("06B03B0B-3B16-4567-9A43-C64D13FDF06F");

        private UniquelyIdentifiableCache<EngineeringChangeObjectState> stateCache;

        public EngineeringChangeObjectState Requested
        {
            get { return this.StateCache.Get(RequestedId); }
        }

        public EngineeringChangeObjectState Noticed
        {
            get { return this.StateCache.Get(NoticedId); }
        }

        public EngineeringChangeObjectState Released
        {
            get { return this.StateCache.Get(ReleasedId); }
        }

        private UniquelyIdentifiableCache<EngineeringChangeObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<EngineeringChangeObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new EngineeringChangeObjectStateBuilder(Session)
                .WithUniqueId(RequestedId)
                .WithName("Requested")
                .Build();

            new EngineeringChangeObjectStateBuilder(Session)
                .WithUniqueId(NoticedId)
                .WithName("Notice")
                .Build();

            new EngineeringChangeObjectStateBuilder(Session)
                .WithUniqueId(ReleasedId)
                .WithName("Released")
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