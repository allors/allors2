namespace Allors.Server
{
    using System;
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class PdfController : BasePdfController
    {
        public PdfController(ISessionService sessionService, IPdfService pdfService)
            : base(sessionService, pdfService)
        {
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public virtual async Task<ActionResult> Download(string id)
        {
            var @object = this.Session.Instantiate(id);
            if (@object == null && Guid.TryParse(id, out Guid uniqueId))
            {
                @object = new UniquelyIdentifiables(this.Session).FindBy(M.Media.UniqueId, uniqueId);
            }

            string html = null;
            string fileName = null;
            string footer = null;
            
//            if (@object is SalesInvoice salesInvoice)
//            {
//                // TODO: Security
//                html = salesInvoice.HtmlContent;
//                fileName = "SalesInvoice-" + salesInvoice.InvoiceNumber + ".pdf";
//                footer = $@"<!DOCTYPE html>
//<html>
//    <style>
//    .footer p {{
//                margin: 3px;
//                font-size: 8px;
//                line-height: 8px;
//                text-align: left;
//                font-weight: normal;
//                padding-bottom: 10px;
//            }}
//    </style>
//    <body>
//        <div class=""footer"">
//                <p>General Conditions upon request, AVIACO BVBA</p>
//                <p>Total amount due in {salesInvoice.PaymentNetDays} days.</p>
//                <p>If payment overdue interest will be calculated according General Conditions.</p>
//        </div>
//    </body>
//</html>";
//            }

            if (html != null)
            {
                return await this.Pdf(html, fileName, null, footer);
            }

            return this.NotFound("Pdf not found");
        }
    }
}
