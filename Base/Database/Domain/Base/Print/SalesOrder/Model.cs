// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesOrderModel
{
    using System.Linq;

    public class Model
    {
        public Model(SalesOrder order)
        {
            var session = order.Strategy.Session;

            this.Order = new OrderModel(order);

            this.TakenBy = new TakenByModel((Organisation)order.TakenBy);
            this.BillTo = new BillToModel(order);
            this.ShipTo = new ShipToModel(order);

            this.OrderItems = order.SalesOrderItems.Select(v => new OrderItemModel(v)).ToArray();
            this.OrderAdjustments = order.OrderAdjustments.Select(v => new OrderAdjustmentModel(v)).ToArray();

            var paymentTerm = new InvoiceTermTypes(session).PaymentNetDays;
            this.SalesTerms = order.SalesTerms.Where(v => !v.TermType.Equals(paymentTerm)).Select(v => new SalesTermModel(v)).ToArray();

            string TakenByCountry = null;
            if (order.TakenBy.PartyContactMechanisms?.FirstOrDefault(v => v.ContactPurposes.Any(p => Equals(p, new ContactMechanismPurposes(session).RegisteredOffice)))?.ContactMechanism is PostalAddress registeredOffice)
            {
                TakenByCountry = registeredOffice.Country.IsoCode;
            }

            if (TakenByCountry == "BE")
            {
                this.VatClause = order.DerivedVatClause?.LocalisedClauses.FirstOrDefault(v => v.Locale.Equals(new Locales(session).DutchBelgium))?.Text;

                if (this.VatClause != null && Equals(order.DerivedVatClause, new VatClauses(session).BeArt14Par2))
                {
                    var shipToCountry = order.DerivedShipToAddress?.Country?.Name;
                    this.VatClause = this.VatClause.Replace("{shipToCountry}", shipToCountry);
                }
            }
        }

        public OrderModel Order { get; }

        public TakenByModel TakenBy { get; }

        public BillToModel BillTo { get; }

        public ShipToModel ShipTo { get; }

        public OrderItemModel[] OrderItems { get; }

        public SalesTermModel[] SalesTerms { get; }

        public OrderAdjustmentModel[] OrderAdjustments { get; }

        public string VatClause { get; }
    }
}
