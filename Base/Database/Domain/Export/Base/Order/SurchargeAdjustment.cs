
// <copyright file="SurchargeAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Meta;

    public partial class SurchargeAdjustment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistPriceableWhereSurchargeAdjustment)
                {
                    var salesInvoiceItem = this.PriceableWhereSurchargeAdjustment as SalesInvoiceItem;
                    var salesOrderItem = this.PriceableWhereSurchargeAdjustment as SalesOrderItem;

                    if (salesInvoiceItem != null)
                    {
                        derivation.AddDependency(this, salesInvoiceItem);
                    }

                    if (salesOrderItem != null)
                    {
                        derivation.AddDependency(this, salesOrderItem);
                    }
                }

                if (this.ExistOrderWhereSurchargeAdjustment)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereSurchargeAdjustment;
                    derivation.AddDependency(this, salesOrder);
                }

                if (this.ExistInvoiceWhereSurchargeAdjustment)
                {
                    var salesInvoice = (SalesInvoice)this.InvoiceWhereSurchargeAdjustment;
                    derivation.AddDependency(this, salesInvoice);
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
