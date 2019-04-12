// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderItemModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.PurchaseOrderModel
{
    public class OrderItemModel
    {
        public OrderItemModel(PurchaseOrderItem item)
        {
            this.Part = item.ExistPart ? item.Part?.Name : item.Description;
            this.Description = item.Description;
            this.Quantity = item.QuantityOrdered;
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("0.00") + " " + currency;
            this.Amount = item.TotalExVat.ToString("0.00") + " " + currency;
            this.Comment = item.Comment;
        }

        public string Part { get; }
        public string Description { get; }
        public decimal Quantity { get; }
        public string Price { get; }
        public string Amount { get; }
        public string Comment { get; }
    }
}
