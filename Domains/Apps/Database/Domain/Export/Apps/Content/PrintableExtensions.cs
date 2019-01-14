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

        private static void UpdatePrintDocument(this Printable @this, byte[] document)
        {
            if (document != null)
            {
                if (@this.ExistPrintDocument)
                {
                    @this.PrintDocument.InData = document;
                }
                else
                {
                    @this.PrintDocument = new MediaBuilder(@this.Strategy.Session).WithInData(document).Build();
                }
            }
            else
            {
                @this.PrintDocument?.Delete();
            }
        }
    }
}