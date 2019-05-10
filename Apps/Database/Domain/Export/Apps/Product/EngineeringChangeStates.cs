// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringChangeObjectStates.cs" company="Allors bvba">
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

    public partial class EngineeringChangeObjectStates
    {
        private static readonly Guid RequestedId = new Guid("1732B578-2CA4-40b5-95B5-6B39D453CF87");
        private static readonly Guid NoticedId = new Guid("811E1661-B788-4c89-BE46-D5DD3B1EE20B");
        private static readonly Guid ReleasedId = new Guid("06B03B0B-3B16-4567-9A43-C64D13FDF06F");

        private UniquelyIdentifiableSticky<EngineeringChangeObjectState> stateCache;

        public EngineeringChangeObjectState Requested => this.StateCache[RequestedId];

        public EngineeringChangeObjectState Noticed => this.StateCache[NoticedId];

        public EngineeringChangeObjectState Released => this.StateCache[ReleasedId];

        private UniquelyIdentifiableSticky<EngineeringChangeObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<EngineeringChangeObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new EngineeringChangeObjectStateBuilder(this.Session)
                .WithUniqueId(RequestedId)
                .WithName("Requested")
                .Build();

            new EngineeringChangeObjectStateBuilder(this.Session)
                .WithUniqueId(NoticedId)
                .WithName("Notice")
                .Build();

            new EngineeringChangeObjectStateBuilder(this.Session)
                .WithUniqueId(ReleasedId)
                .WithName("Released")
                .Build();
        }
    }
}