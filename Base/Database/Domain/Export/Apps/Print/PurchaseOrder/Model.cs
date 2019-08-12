// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="Allors bvba">
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
    using System.Linq;

    public class Model
    {
        public Model(PurchaseOrder order)
        {
            this.Order = new OrderModel(order);
            
            this.OrderedBy = new OrderedByModel((Organisation)order.OrderedBy);
            this.TakenVia = new TakenViaModel(order);
            this.ShipTo = new ShipToModel(order);

            this.OrderItems = order.PurchaseOrderItems.Select(v => new OrderItemModel(v)).ToArray();
        }

        public OrderModel Order { get; }

        public OrderedByModel OrderedBy { get; }

        public TakenViaModel TakenVia { get; }

        public ShipToModel ShipTo { get; }

        public OrderItemModel[] OrderItems { get; }
    }
}
