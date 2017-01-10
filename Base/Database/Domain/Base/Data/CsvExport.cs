// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvExport.cs" company="Allors bvba">
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
    using Allors;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public class CsvExport
    {
        public CsvExport(string name)
        {
            this.Name = name;
            this.Columns = new List<CsvExportColumn>();
        }

        public string Name { get; private set; }

        public string RecordSeparator { get; set; }

        public string FieldSeparator { get; set; }

        public string SubfieldSeparator { get; set; }

        public List<CsvExportColumn> Columns { get; private set; }

        public static string Escape(string value)
        {
            return value == null ? null : "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        public string Write(Extent extent, Domain.Locale locale, IAccessControlListFactory aclFactory)
        {
            return this.Write(extent, new CultureInfo(locale.Name), aclFactory);
        }

        public string Write(Extent extent, CultureInfo cultureInfo, IAccessControlListFactory aclFactory)
        {
            var actualRecordSeparator = this.RecordSeparator ?? Environment.NewLine;
            var actualFieldSeparator = this.FieldSeparator ?? cultureInfo.TextInfo.ListSeparator;

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < this.Columns.Count; i++)
            {
                var column = this.Columns[i];
                stringBuilder.Append("\"");
                stringBuilder.Append(column.Header);
                stringBuilder.Append("\"");

                if (i < this.Columns.Count - 1)
                {
                    stringBuilder.Append(actualFieldSeparator);
                }
            }

            stringBuilder.Append(Environment.NewLine);

            for (var objectCounter = 0; objectCounter < extent.Count; objectCounter++)
            {
                var obj = extent[objectCounter];

                for (var columnCounter = 0; columnCounter < this.Columns.Count; columnCounter++)
                {
                    var column = this.Columns[columnCounter];
                    column.Write(this, cultureInfo, stringBuilder, obj, aclFactory);

                    if (columnCounter < this.Columns.Count - 1)
                    {
                        stringBuilder.Append(actualFieldSeparator);
                    }
                }

                if (objectCounter < extent.Count - 1)
                {
                    stringBuilder.Append(actualRecordSeparator);
                }
            }

            return stringBuilder.ToString();
        }
    }
}