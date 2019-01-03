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

namespace Allors.Domain.SalesInvoicePrint
{
    public class InvoiceItemModel
    {
        public string Product;
        public string Description;
        public decimal Quantity;
        public decimal Price;
        public decimal Amount;

        public InvoiceItemModel(SalesInvoiceItem invoiceItem)
        {
            this.Product = invoiceItem.Product?.Id.ToString();
            this.Description = invoiceItem.Description;
            this.Quantity = invoiceItem.Quantity;
            this.Price = invoiceItem.ActualUnitPrice ?? 0.0m;
            this.Amount = this.Quantity * this.Price;
        }
    }
}
