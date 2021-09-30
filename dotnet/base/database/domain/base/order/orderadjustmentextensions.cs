// <copyright file="OrderAdjustmentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public static partial class OrderAdjustmentExtensions
    {
        public static void BaseOnPreDerive(this OrderAdjustment @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.ChangeSet.Associations.Contains(@this.Id))
            {
                if (@this.ExistOrderWhereOrderAdjustment)
                {
                    var order = @this.OrderWhereOrderAdjustment;
                    iteration.AddDependency(@this, order);
                    iteration.Mark(order);
                }

                if (@this.ExistInvoiceWhereOrderAdjustment)
                {
                    var invoice = @this.InvoiceWhereOrderAdjustment;
                    iteration.AddDependency(@this, invoice);
                    iteration.Mark(invoice);
                }

                if (@this.ExistQuoteWhereOrderAdjustment)
                {
                    var quote = @this.QuoteWhereOrderAdjustment;
                    iteration.AddDependency(@this, quote);
                    iteration.Mark(quote);
                }
            }
        }

        public static void BaseOnDerive(this OrderAdjustment @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(@this, M.OrderAdjustment.Amount, M.ShippingAndHandlingCharge.Percentage);
            derivation.Validation.AssertExistsAtMostOne(@this, M.OrderAdjustment.Amount, M.ShippingAndHandlingCharge.Percentage);
        }
    }
}
