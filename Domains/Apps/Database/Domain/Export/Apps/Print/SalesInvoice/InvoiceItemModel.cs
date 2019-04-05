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

namespace Allors.Domain.Print.SalesInvoiceModel
{
    public class InvoiceItemModel
    {
        public InvoiceItemModel(SalesInvoiceItem item)
        {
            this.Reference = item.InvoiceItemType?.Name;
            this.Product = item.ExistProduct ? item.Product?.Name : item.Part?.Name;
            this.Description = item.Description;
            this.Details = item.Details;
            this.Quantity = item.Quantity;
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("0.00") + " " + currency;
            this.Amount = item.TotalExVat.ToString("0.00") + " " + currency;
            this.Comment = item.Comment;
        }

        public string Reference { get; }
        public string Product { get; }
        public string Description { get; }
        public string Details { get; }
        public decimal Quantity { get; }
        public string Price { get; }
        public string Amount { get; }
        public string Comment { get; }
    }
}
