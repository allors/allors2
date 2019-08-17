// <copyright file="WorksheetExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using System.Linq;

    using Microsoft.Office.Interop.Excel;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public static class WorksheetExtensions
    {
        public static object GetCustomPropertyValue(this Worksheet worksheet, string name)
        {
            var customProperty = worksheet
                .CustomProperties
                .Cast<CustomProperty>()
                .FirstOrDefault(v => name.Equals(v.Name));

            return customProperty?.Value;
        }

        public static void SetCustomPropertyValue(this Worksheet worksheet, string name, object value)
        {
            var customProperty = worksheet
                .CustomProperties
                .Cast<CustomProperty>()
                .FirstOrDefault(v => name.Equals(v.Name));

            if (customProperty == null)
            {
                worksheet.CustomProperties.Add("AllorsType", value);
            }
            else
            {
                customProperty.Value = value;
            }
        }
    }
}
