// <copyright file="PackagingContent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
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
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.ChangeSet.Associations.Contains(this.Id))
            {
                iteration.AddDependency(this.ShipmentItem, this);
                iteration.Mark(this.ShipmentItem);
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
