// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvExportColumn.cs" company="Allors bvba">
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
    using System.Globalization;
    using System.Text;

    using Allors;

    public abstract class CsvExportColumn
    {
        public string Header { get; set; }

        public abstract void Write(CsvExport file, CultureInfo cultureInfo, StringBuilder stringBuilder, IObject obj, IAccessControlListFactory aclFactory);
    }
}