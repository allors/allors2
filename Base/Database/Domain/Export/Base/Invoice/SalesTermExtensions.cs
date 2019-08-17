// <copyright file="SalesTermExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public static partial class SalesTermExtensions
    {
        public static void BaseOnPreDerive(this SalesTerm @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (@this.ExistInvoiceWhereSalesTerm)
            {
                derivation.AddDependency(@this.InvoiceWhereSalesTerm, @this);
            }

            if (@this.ExistOrderWhereSalesTerm)
            {
                derivation.AddDependency(@this.OrderWhereSalesTerm, @this);
            }
        }
    }
}
