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
    using System.Linq;
    using System.Reflection;

    public static partial class PrintableExtensions
    {
        public static void RenderPrintDocument(this Printable @this, Template template, object model)
        {
            if (model is IReadOnlyDictionary<string, object> alreadyDictionary)
            {
                RenderPrintDocument(@this, template, alreadyDictionary);
            }
            else
            {
                var dictionary = model.GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .ToDictionary(propInfo => propInfo.Name, propInfo => propInfo.GetValue(model, null));

                RenderPrintDocument(@this, template, dictionary);
            }
        }

        public static void RenderPrintDocument(this Printable @this, Template template, IReadOnlyDictionary<string, object> model)
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