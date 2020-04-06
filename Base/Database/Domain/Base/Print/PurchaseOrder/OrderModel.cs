// <copyright file="OrderModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseOrderModel
{
    using System.Globalization;

    public class OrderModel
    {
        public OrderModel(PurchaseOrder order)
        {
            this.Description = order.Description;
            this.Number = order.OrderNumber;
            this.Date = order.OrderDate.ToString("yyyy-MM-dd");
            this.CustomerReference = order.CustomerReference;

            // TODO: Where does the currency come from?
            var currency = "€";
            this.SubTotal = order.TotalBasePrice.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.TotalExVat = order.TotalExVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.VatCharge = order.VatRegime?.VatRate?.Rate.ToString("n2");
            this.TotalVat = order.TotalVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.TotalIncVat = order.TotalIncVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
        }

        public string Description { get; }

        public string Number { get; }

        public string Date { get; }

        public string CustomerReference { get; }

        public string SubTotal { get; }

        public string TotalExVat { get; }

        public string VatCharge { get; }

        public string TotalVat { get; }

        public string TotalIncVat { get; }
    }
}
