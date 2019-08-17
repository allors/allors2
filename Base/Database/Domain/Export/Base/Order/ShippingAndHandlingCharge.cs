// <copyright file="ShippingAndHandlingCharge.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Meta;

    public partial class ShippingAndHandlingCharge
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistOrderWhereShippingAndHandlingCharge)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereShippingAndHandlingCharge;
                    derivation.AddDependency(this, salesOrder);
                }

                if (this.ExistInvoiceWhereShippingAndHandlingCharge)
                {
                    var salesInvoice = (SalesInvoice)this.InvoiceWhereShippingAndHandlingCharge;
                    derivation.AddDependency(this, salesInvoice);
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
