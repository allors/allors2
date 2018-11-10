// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedInventoryItem.cs" company="Allors bvba">
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
    using System.Linq;
    using System.Text;

    using Meta;

    public partial class SerialisedInventoryItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            { new TransitionalConfiguration(M.SerialisedInventoryItem, M.SerialisedInventoryItem.SerialisedInventoryItemState), };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public int QuantityOnHand
            => this.Part.GetInventoryStrategy.OnHandSerialisedStates.Contains(this.SerialisedInventoryItemState) ? 1 : 0;

        public int AvailableToPromise
            => this.Part.GetInventoryStrategy.AvailableToPromiseSerialisedStates.Contains(this.SerialisedInventoryItemState) ? 1 : 0;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSerialisedInventoryItemState)
            {
                this.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Available;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistFacility && this.ExistPart && this.Part.ExistDefaultFacility)
            {
                this.Facility = this.Part.DefaultFacility;
            }

            if (!this.ExistName && this.ExistPart && this.Part.ExistName)
            {
                this.Name = this.Part.Name;
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            foreach (InventoryItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }
    }
}