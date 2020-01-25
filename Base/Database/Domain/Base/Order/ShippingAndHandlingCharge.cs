// <copyright file="ShippingAndHandlingCharge.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class ShippingAndHandlingCharge
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistOrderWhereShippingAndHandlingCharge)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereShippingAndHandlingCharge;
                    iteration.AddDependency(this, salesOrder);
                    iteration.Mark(salesOrder);
                }

                if (this.ExistInvoiceWhereShippingAndHandlingCharge)
                {
                    var salesInvoice = (SalesInvoice)this.InvoiceWhereShippingAndHandlingCharge;
                    iteration.AddDependency(this, salesInvoice);
                    iteration.Mark(salesInvoice);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.ShippingAndHandlingCharge.Amount, M.ShippingAndHandlingCharge.Percentage);
            derivation.Validation.AssertExistsAtMostOne(this, M.ShippingAndHandlingCharge.Amount, M.ShippingAndHandlingCharge.Percentage);
        }
    }
}
