// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceItemModel.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain.Print.PurchaseInvoiceModel
{
    public class InvoiceItemModel
    {
        public InvoiceItemModel(PurchaseInvoiceItem item)
        {
            this.Part = item.ExistPart ? item.Part?.Name : item.Description;
            this.Description = item.ExistPart ? item.Description : string.Empty;
            this.Quantity = item.Quantity;
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("0.00") + " " + currency;
            this.Amount = item.TotalExVat.ToString("0.00") + " " + currency;
            this.Comment = item.Comment;
            this.SupplierProductId = item.Part?.SupplierOfferingsWherePart?.FirstOrDefault(v => v.Supplier.Equals(item.PurchaseInvoiceWherePurchaseInvoiceItem.BilledFrom))?.SupplierProductId;
        }

        public string Part { get; }
        public string Description { get; }
        public decimal Quantity { get; }
        public string Price { get; }
        public string Amount { get; }
        public string Comment { get; }
        public string SupplierProductId { get; }
    }
}
