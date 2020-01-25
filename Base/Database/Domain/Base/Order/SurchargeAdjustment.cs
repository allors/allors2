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

                if (this.ExistOrderWhereSurchargeAdjustment)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereSurchargeAdjustment;
                    iteration.AddDependency(this, salesOrder);
                    iteration.Mark(salesOrder);
                }

                if (this.ExistInvoiceWhereSurchargeAdjustment)
                {
                    var salesInvoice = (SalesInvoice)this.InvoiceWhereSurchargeAdjustment;
                    iteration.AddDependency(this, salesInvoice);
                    iteration.Mark(salesInvoice);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.SurchargeAdjustment.Amount, M.SurchargeAdjustment.Percentage);
            derivation.Validation.AssertExistsAtMostOne(this, M.SurchargeAdjustment.Amount, M.SurchargeAdjustment.Percentage);
        }
    }
}
