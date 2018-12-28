// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoice.cs" company="Allors bvba">
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

namespace Allors.Domain.Print
{
    using System.Collections.Generic;

    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;
    using Sandwych.Reporting;

    class SalesInvoicePrint
    {
        public string Description;

        public string Number;

        public ImageBlob Barcode;

        public Dictionary<string, object> Model { get; }

        public SalesInvoicePrint(SalesInvoice salesInvoice)
        {
            this.Description = salesInvoice.Description;
            this.Number = salesInvoice.InvoiceNumber;

            if (salesInvoice.ExistInvoiceNumber)
            {
                var session = salesInvoice.Strategy.Session;
                var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                var barcode = barcodeService.Generate(this.Number, BarcodeType.CODE_128, 320, 80);
                this.Barcode = new ImageBlob("png", barcode);
            }

            this.Model = new Dictionary<string, object> { { "invoice", this } };

            // Logo
            var singleton = salesInvoice.Strategy.Session.GetSingleton();
            ImageBlob logo = new ImageBlob("png", singleton.LogoImage.MediaContent.Data);

            if (salesInvoice.ExistBilledFrom && salesInvoice.BilledFrom.ExistLogoImage)
            {
                logo = new ImageBlob("png", salesInvoice.BilledFrom.LogoImage.MediaContent.Data);
            }
            this.Model.Add("logo", logo);

            // Billed From
            if (salesInvoice.ExistBilledFrom)
            {
                this.Model.Add("billFrom", new SalesInvoiceBillFromPrint(salesInvoice.BilledFrom));
            }

            // Sales Invoice Items
            var invoiceItems = new List<SalesInvoiceItemPrint>();

            foreach (SalesInvoiceItem salesInvoiceItem in salesInvoice.InvoiceItems)
            {
                invoiceItems.Add(new SalesInvoiceItemPrint(salesInvoiceItem));
            }

            if (invoiceItems.Count > 0)
            {
                this.Model.Add("items", invoiceItems.ToArray());
            }
        }
    }
}
