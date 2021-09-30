// <copyright file="OrderModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesOrderModel
{
    using System;
    using System.Globalization;

    public class OrderModel
    {
        public OrderModel(SalesOrder order)
        {
            var currencyIsoCode = order.DerivedCurrency.IsoCode;

            this.Description = order.Description?.Split('\n');
            this.Currency = currencyIsoCode;
            this.Number = order.OrderNumber;
            this.Date = order.OrderDate.ToString("yyyy-MM-dd");
            DateTime? ret;
            if (order.ExistOrderDate)
            {
                ret = order.OrderDate.AddDays(order.PaymentNetDays);
            }
            else
            {
                ret = null;
            }

            this.DueDate = ret?.ToString("yyyy-MM-dd");
            this.CustomerReference = order.CustomerReference;

            // TODO: Where does the currency come from?
            this.SubTotal = order.TotalBasePrice.ToString("N2", new CultureInfo("nl-BE"));
            this.TotalExVat = order.TotalExVat.ToString("N2", new CultureInfo("nl-BE"));
            this.VatRate = order.DerivedVatRate?.Rate.ToString("n2");
            this.TotalVat = order.TotalVat.ToString("N2", new CultureInfo("nl-BE"));

            // IRPF is subtracted for total amount to pay
            var totalIrpf = order.TotalIrpf * -1;
            this.IrpfRate = order.DerivedIrpfRate?.Rate.ToString("n2");
            this.TotalIrpf = totalIrpf.ToString("N2", new CultureInfo("nl-BE"));
            this.PrintIrpf = order.TotalIrpf != 0;

            this.TotalIncVat = order.TotalIncVat.ToString("N2", new CultureInfo("nl-BE"));
            this.GrandTotal = currencyIsoCode + " " + order.GrandTotal.ToString("N2", new CultureInfo("nl-BE"));

            this.PaymentNetDays = order.PaymentNetDays;
        }

        public string[] Description { get; }

        public string Number { get; }

        public string Date { get; }

        public string DueDate { get; }

        public string CustomerReference { get; }

        public string SubTotal { get; }

        public string TotalExVat { get; }

        public string VatRate { get; }

        public string TotalVat { get; }

        public string IrpfRate { get; }

        public string TotalIrpf { get; }

        public string TotalIncVat { get; }

        public string GrandTotal { get; }

        public int PaymentNetDays { get; }

        public string Currency { get; }

        public bool PrintIrpf { get; }
    }
}
