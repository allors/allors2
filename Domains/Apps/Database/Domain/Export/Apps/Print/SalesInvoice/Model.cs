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

namespace Allors.Domain.SalesInvoicePrint
{
    using System.Linq;

    using Allors.Domain.Print;
    using Sandwych.Reporting;

    public class Model
    {
        public Model(SalesInvoice salesInvoice)
        {
            this.Invoice = new InvoiceModel(salesInvoice);

            if (salesInvoice.ExistBilledFrom && salesInvoice.BilledFrom.ExistLogoImage)
            {
                this.Logo = new ImageBlob("png", salesInvoice.BilledFrom.LogoImage.MediaContent.Data);
            }
            else
            {
                var singleton = salesInvoice.Strategy.Session.GetSingleton();
                this.Logo = new ImageBlob("png", singleton.LogoImage.MediaContent.Data);
            }

            this.BilledFrom = new BilledFromModel((Organisation)salesInvoice.BilledFrom);
            this.InvoiceItems = salesInvoice.SalesInvoiceItems.Select(v => new InvoiceItemModel(v)).ToArray();
        }

        public InvoiceModel Invoice { get; }

        public ImageBlob Logo { get; }

        public BilledFromModel BilledFrom { get; }

        public InvoiceItemModel[] InvoiceItems { get; }
    }
}
