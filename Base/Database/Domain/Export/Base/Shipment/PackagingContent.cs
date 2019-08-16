
// <copyright file="PackagingContent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Meta;
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

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                derivation.AddDependency(this.ShipmentItem, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistQuantity && this.ExistShipmentItem)
            {
                var maxQuantity = this.ShipmentItem.Quantity - this.ShipmentItem.QuantityShipped;
                if (this.Quantity == 0 || this.Quantity > maxQuantity)
                {
                    derivation.Validation.AddError(this, M.PackagingContent.Quantity, ErrorMessages.PackagingContentMaximum);
                }
            }
        }
    }
}
