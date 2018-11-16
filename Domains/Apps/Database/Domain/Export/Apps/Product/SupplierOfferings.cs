// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierOfferings.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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