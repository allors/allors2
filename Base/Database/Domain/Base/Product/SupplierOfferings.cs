// <copyright file="SupplierOfferings.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SupplierOfferings
    {
        public decimal PurchasePrice(Party supplier, DateTime orderDate, Part part = null)
        {
            decimal price = 0;

            foreach (SupplierOffering supplierOffering in supplier.SupplierOfferingsWhereSupplier)
            {
                if (supplierOffering.ExistPart && supplierOffering.Part.Equals(part))
                {
                    if (supplierOffering.FromDate <= orderDate && (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= orderDate))
                    {
                        price = supplierOffering.Price;
                        break;
                    }
                }
            }

            return price;
        }
    }
}
