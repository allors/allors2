// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvExportFunction.cs" company="Allors bvba">
//   Copyright 2002-2011 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Globalization;
    using System.Text;

    using Allors;

    public class CsvExportFunction<T> : CsvExportColumn where T : IObject
    {
        public CsvExportFunction(string header, Func<T, IAccessControlListFactory, string> function)
        {
            this.Header = header;
            this.ObjectFunction = function;
        }

        public CsvExportFunction(string header, Func<T, IAccessControlListFactory, CultureInfo, string> function)
        {
            this.Header = header;
            this.ObjectAndCultureInfoFunction = function;
        }

        public bool AutoEscape { get; set; }

        private Func<T, IAccessControlListFactory, string> ObjectFunction { get; set; }

        private Func<T, IAccessControlListFactory, CultureInfo, string> ObjectAndCultureInfoFunction { get; set; }

        public override void Write(CsvExport file, CultureInfo cultureInfo, StringBuilder stringBuilder, IObject obj, IAccessControlListFactory aclFactory)
        {
            var value = this.ObjectFunction != null ? this.ObjectFunction((T)obj, aclFactory) : this.ObjectAndCultureInfoFunction((T)obj, aclFactory, cultureInfo);

            if (this.AutoEscape)
            {
                value = CsvExport.Escape(value);
            }

            stringBuilder.Append(value);
        }
    }
}