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

namespace Allors.Domain.Print.SalesInvoiceModel
{
    public class InvoiceModel
    {
        public InvoiceModel(SalesInvoice invoice)
        {
            this.Description = invoice.Description;
            this.Number = invoice.InvoiceNumber;
            this.Date = invoice.InvoiceDate.ToString("yyyy-MM-dd");
            this.DueDate = invoice.DueDate?.ToString("yyyy-MM-dd");
            this.CustomerReference = invoice.CustomerReference;

            // TODO: Where does the currency come from?
            var currency = "€";
            this.SubTotal = invoice.TotalBasePrice.ToString("0.00") + " " + currency;
            this.Deposit = invoice.AmountPaid.ToString("0.00") + " " + currency;
            this.TotalExVat = invoice.TotalExVat.ToString("0.00") + " " + currency;
            this.VatCharge = invoice.VatRegime?.VatRate?.Rate.ToString("n2");
            this.TotalVat = invoice.TotalVat.ToString("0.00") + " " + currency;
            this.TotalIncVat = invoice.TotalIncVat.ToString("0.00") + " " + currency;

            this.PaymentNetDays = invoice.PaymentNetDays;
        }

        public string Description { get; }
        public string Number { get; }
        public string Date { get; }
        public string DueDate { get; }
        public string CustomerReference { get; }
        public string SubTotal { get; }
        public string Deposit { get; }
        public string TotalExVat { get; }
        public string VatCharge { get; }
        public string TotalVat { get; }
        public string TotalIncVat { get; }
        public int PaymentNetDays { get; }
    }
}
