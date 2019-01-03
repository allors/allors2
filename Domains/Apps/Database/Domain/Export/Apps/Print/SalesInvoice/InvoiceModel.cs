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

namespace Allors.Domain.Print
{
    using System.Xml.Serialization;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Sandwych.Reporting;

    public class InvoiceModel
    {
        public InvoiceModel(SalesInvoice salesInvoice)
        {
            this.Description = salesInvoice.Description;
            this.Number = salesInvoice.InvoiceNumber;
            if (salesInvoice.ExistInvoiceNumber)
            {
                var session = salesInvoice.Strategy.Session;
                var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                var barcode = barcodeService.Generate(salesInvoice.InvoiceNumber, BarcodeType.CODE_128, 320, 80);
                this.Barcode = new ImageBlob("png", barcode);
            }
        }

        public string Description { get; }
        public string Number { get; }
        public ImageBlob Barcode { get; }
    }
}
