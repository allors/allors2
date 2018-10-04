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
    using System.IO;

    public static partial class PrintableExtensions
    {
        public static void RenderPrintDocument(this Printable @this, Template template, Dictionary<string, object> model)
        {
            var document = template?.Render(model);

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


            //if (document != null)
            //{
            //    File.WriteAllBytes($@"\temp\{@this.Strategy.Class.Name}_{@this.Strategy.ObjectId}.odt", document);
            //}
        }
    }
}