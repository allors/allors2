// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceModel.cs" company="Allors bvba">
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
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Sandwych.Reporting;

    public class OrderModel
    {
        public OrderModel(SalesOrder order)
        {
            this.Description = order.Description;
            this.Number = order.OrderNumber;
            if (order.ExistOrderNumber)
            {
                var session = order.Strategy.Session;
                var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                var barcode = barcodeService.Generate(order.OrderNumber, BarcodeType.CODE_128, 320, 80);
                this.Barcode = new ImageBlob("png", barcode);
            }

            this.Date = order.OrderDate.ToString("yyyy-MM-dd");
            this.DueDate = order.DueDate?.ToString("yyyy-MM-dd");
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
        public ImageBlob Barcode { get; }
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
