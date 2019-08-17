// <copyright file="Sheets.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using Allors.Excel.People;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Sheets
    {
        public PeopleSheet CreatePeople()
        {
            var worksheet = this.Host.ActiveWorksheet;
            worksheet.SetCustomPropertyValue("AllorsType", nameof(PeopleSheet));

            var sheet = new PeopleSheet(this, worksheet);
            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }

        private Sheet Instantiate(Worksheet worksheet)
        {
            var allorsType = (string)worksheet.GetCustomPropertyValue("AllorsType");

            switch (allorsType)
            {
                case nameof(PeopleSheet):
                    return new PeopleSheet(this, worksheet);
            }

            return null;
        }
    }
}
