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

namespace Allors.Domain.SalesOrderPrint
{
    using System.Linq;
    using Sandwych.Reporting;

    public class Model
    {
        public Model(SalesOrder order)
        {
            this.Order = new OrderModel(order);

            if (order.TakenBy?.ExistLogoImage == true)
            {
                this.Logo = new ImageBlob("png", order.TakenBy.LogoImage.MediaContent.Data);
            }
            else
            {
                var singleton = order.Strategy.Session.GetSingleton();
                this.Logo = new ImageBlob("png", singleton.LogoImage.MediaContent.Data);
            }

            this.TakenBy = new TakenByModel((Organisation)order.TakenBy);
            this.BillTo = new BillToModel(order);
            this.ShipTo = new ShipToModel(order);

            this.OrderItems = order.SalesOrderItems.Select(v => new OrderItemModel(v)).ToArray();

            var paymentTerm = new InvoiceTermTypes(order.Strategy.Session).PaymentNetDays;
            this.SalesTerms = order.SalesTerms.Where(v => !v.TermType.Equals(paymentTerm)).Select(v => new SalesTermModel(v)).ToArray();
        }

        public OrderModel Order { get; }

        public ImageBlob Logo { get; }

        public TakenByModel TakenBy { get; }

        public BillToModel BillTo { get; }

        public ShipToModel ShipTo { get; }

        public OrderItemModel[] OrderItems { get; }

        public SalesTermModel[] SalesTerms { get; }
    }
}
