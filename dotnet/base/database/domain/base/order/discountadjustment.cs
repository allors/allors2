// <copyright file="DiscountAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class DiscountAdjustment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistPriceableWhereDiscountAdjustment)
                {
                    if (this.PriceableWhereDiscountAdjustment is SalesInvoiceItem salesInvoiceItem)
                    {
                        iteration.AddDependency(this, salesInvoiceItem);
                        iteration.Mark(salesInvoiceItem);
                    }

                    if (this.PriceableWhereDiscountAdjustment is SalesOrderItem salesOrderItem)
                    {
                        iteration.AddDependency(this, salesOrderItem);
                        iteration.Mark(salesOrderItem);
                    }
                }
            }
        }
    }
}
