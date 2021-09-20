// <copyright file="SalesTermExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class SalesTermExtensions
    {
        public static void BaseOnPreDerive(this SalesTerm @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                if (@this.ExistInvoiceWhereSalesTerm)
                {
                    iteration.AddDependency(@this.InvoiceWhereSalesTerm, @this);
                    iteration.Mark(@this.InvoiceWhereSalesTerm);
                }

                if (@this.ExistOrderWhereSalesTerm)
                {
                    iteration.AddDependency(@this.OrderWhereSalesTerm, @this);
                    iteration.Mark(@this.OrderWhereSalesTerm);
                }
            }
        }
    }
}
