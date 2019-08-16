// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintableExtensions.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    using System.Collections.Generic;

    public static partial class PrintableExtensions
    {
        public static void RenderPrintDocument(this Printable @this, Template template, object model, IDictionary<string, byte[]> images)
        {
            var document = template?.Render(model, images);
            @this.UpdatePrintDocument(document);
        }

        public static void RenderPrintDocument(this Printable @this, Template template, IDictionary<string, object> model, IDictionary<string, byte[]> images)
        {
            var document = template?.Render(model, images);
            @this.UpdatePrintDocument(document);
        }

        public static void ResetPrintDocument(this Printable @this)
        {
            if (!@this.ExistPrintDocument)
            {
                @this.PrintDocument = new PrintDocumentBuilder(@this.Strategy.Session).Build();
            }

            @this.PrintDocument.Media?.Delete();
        }

        private static void UpdatePrintDocument(this Printable @this, byte[] document)
        {
            if (document != null)
            {
                if (!@this.ExistPrintDocument)
                {
                    @this.PrintDocument = new PrintDocumentBuilder(@this.Strategy.Session).Build();
                }

                if (!@this.PrintDocument.ExistMedia)
                {
                    @this.PrintDocument.Media = new MediaBuilder(@this.Strategy.Session).Build();
                }

                @this.PrintDocument.Media.InData = document;
            }
            else
            {
                @this.ResetPrintDocument();
            }
        }
    }
}
