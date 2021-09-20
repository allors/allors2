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
            this.Description = order.Description?.Split('\n');
            this.Number = order.OrderNumber;
            this.Date = order.OrderDate.ToString("yyyy-MM-dd");
            this.CustomerReference = order.CustomerReference;

            this.SubTotal = order.TotalBasePrice.ToString("N2", new CultureInfo("nl-BE"));
            this.TotalExVat = order.TotalExVat.ToString("N2", new CultureInfo("nl-BE"));
            this.VatRate = order.DerivedVatRate?.Rate.ToString("n2");
            this.TotalVat = order.TotalVat.ToString("N2", new CultureInfo("nl-BE"));
            this.IrpfRate = order.DerivedIrpfRate?.Rate.ToString("n2");
            this.TotalIrpf = order.TotalIrpf.ToString("N2", new CultureInfo("nl-BE"));
            this.TotalIncVat = order.TotalIncVat.ToString("N2", new CultureInfo("nl-BE"));

            var currencyIsoCode = order.DerivedCurrency.IsoCode;
            this.GrandTotal = currencyIsoCode + " " + order.GrandTotal.ToString("N2", new CultureInfo("nl-BE"));
        }

        public string[] Description { get; }

        public string Number { get; }

        public string Date { get; }

        public string CustomerReference { get; }

        public string SubTotal { get; }

        public string TotalExVat { get; }

        public string VatRate { get; }

        public string TotalVat { get; }

        public string IrpfRate { get; }

        public string TotalIrpf { get; }

        public string TotalIncVat { get; }

        public string GrandTotal { get; }
    }
}
