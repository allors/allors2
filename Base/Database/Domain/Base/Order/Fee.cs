// <copyright file="Fee.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Fee
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistOrderWhereFee)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereFee;
                    iteration.AddDependency(this, salesOrder);
                    iteration.Mark(salesOrder);
                }

                if (this.ExistInvoiceWhereFee)
                {
                    var salesInvoice = (SalesInvoice)this.InvoiceWhereFee;
                    iteration.AddDependency(this, salesInvoice);
                    iteration.Mark(salesInvoice);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.Fee.Amount, M.Fee.Percentage);
            derivation.Validation.AssertExistsAtMostOne(this, M.Fee.Amount, M.Fee.Percentage);
        }
    }
}
