// <copyright file="SurchargeAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class SurchargeAdjustment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistPriceableWhereSurchargeAdjustment)
                {
                    if (this.PriceableWhereSurchargeAdjustment is SalesInvoiceItem salesInvoiceItem)
                    {
                        iteration.AddDependency(this, salesInvoiceItem);
                        iteration.Mark(salesInvoiceItem);
                    }

                    if (this.PriceableWhereSurchargeAdjustment is SalesOrderItem salesOrderItem)
                    {
                        iteration.AddDependency(this, salesOrderItem);
                        iteration.Mark(salesOrderItem);
                    }
                }
            }
        }
    }
}
