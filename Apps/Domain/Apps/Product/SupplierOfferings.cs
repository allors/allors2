// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierOfferings.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class SupplierOfferings
    {
        public ProductPurchasePrice PurchasePrice(Party supplier, DateTime orderDate, Product product = null, Part part = null)
        {
            ProductPurchasePrice purchasePrice = null;

            foreach (SupplierOffering supplierOffering in supplier.SupplierOfferingsWhereSupplier)
            {
                if ((supplierOffering.ExistProduct && supplierOffering.Product.Equals(product)) ||
                    (supplierOffering.ExistPart && supplierOffering.Part.Equals(part)))
                {
                    if (supplierOffering.FromDate <= orderDate && (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= orderDate))
                    {
                        foreach (ProductPurchasePrice productPurchasePrice in supplierOffering.ProductPurchasePrices)
                        {
                            if (productPurchasePrice.FromDate <= orderDate && (!productPurchasePrice.ExistThroughDate || productPurchasePrice.ThroughDate >= orderDate))
                            {
                                purchasePrice = productPurchasePrice;
                                break;
                            }
                        }
                    }                    
                }   

                if (purchasePrice != null)
                {
                    break;
                }
            }

            return purchasePrice;
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}