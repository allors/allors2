// <copyright file="OrderModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesOrderModel
{
    using System;

    public class OrderModel
    {
        public OrderModel(SalesOrder order)
        {
            this.Description = order.Description;
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
            var currency = "€";
            this.SubTotal = order.TotalBasePrice.ToString("0.00") + " " + currency;
            this.TotalExVat = order.TotalExVat.ToString("0.00") + " " + currency;
            this.VatCharge = order.VatRegime?.VatRate?.Rate.ToString("n2");
            this.TotalVat = order.TotalVat.ToString("0.00") + " " + currency;
            this.TotalIncVat = order.TotalIncVat.ToString("0.00") + " " + currency;

            this.PaymentNetDays = order.PaymentNetDays;
        }

        public string Description { get; }
        public string Number { get; }
        public string Date { get; }
        public string DueDate { get; }
        public string CustomerReference { get; }
        public string SubTotal { get; }
        public string TotalExVat { get; }
        public string VatCharge { get; }
        public string TotalVat { get; }
        public string TotalIncVat { get; }
        public int PaymentNetDays { get; }
    }
}
