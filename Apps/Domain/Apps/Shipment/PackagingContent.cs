// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackagingContent.cs" company="Allors bvba">
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
    using Resources;

    public partial class PackagingContent
    {
        public string ContentDescription
        {
            get
            {
                if (this.ShipmentItem.ExistGood)
                {
                    return this.ShipmentItem.Good.Name;
                }

                if (this.ShipmentItem.ExistPart)
                {
                    return this.ShipmentItem.Part.Name;
                }

                return this.ShipmentItem.ContentsDescription;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                derivation.AddDependency(this.ShipmentItem, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistQuantity && this.ExistShipmentItem)
            {
                var maxQuantity = this.ShipmentItem.Quantity - this.ShipmentItem.QuantityShipped;
                if (this.Quantity == 0 || this.Quantity > maxQuantity)
                {
                    derivation.Log.AddError(this, PackagingContents.Meta.Quantity, ErrorMessages.PackagingContentMaximum);
                }
            }
        }
    }
}