// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssetAssignmentStatuses.cs" company="Allors bvba">
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

    public partial class AssetAssignmentStatuses
    {
        private static readonly Guid RequestedId = new Guid("9CF35CC2-9E16-4c8a-A2F5-2D2DDD056AED");
        private static readonly Guid AssignedId = new Guid("7CA979A0-8CBF-426f-AFD2-F5C519FB206D");

        private UniquelyIdentifiableSticky<AssetAssignmentStatus> cache;

        public AssetAssignmentStatus Requested => this.Cache[RequestedId];

        public AssetAssignmentStatus Assigned => this.Cache[AssignedId];

        private UniquelyIdentifiableSticky<AssetAssignmentStatus> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<AssetAssignmentStatus>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new AssetAssignmentStatusBuilder(this.Session)
                .WithName("Requested")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aangevraagd").WithLocale(dutchLocale).Build())
                .WithUniqueId(RequestedId)
                .Build();
            
            new AssetAssignmentStatusBuilder(this.Session)
                .WithName("Assigned")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Toegekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(AssignedId)
                .Build();
        }
    }
}
