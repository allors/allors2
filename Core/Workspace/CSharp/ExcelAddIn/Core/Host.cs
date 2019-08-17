// <copyright file="Host.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;

    using Microsoft.Office.Tools;
    using Microsoft.Office.Tools.Excel;

    using Application = Microsoft.Office.Interop.Excel.Application;
    using ListObject = Microsoft.Office.Tools.Excel.ListObject;
    using Workbook = Microsoft.Office.Tools.Excel.Workbook;
    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Host
    {
        public Host(Application application, CustomTaskPaneCollection customTaskPanes, ApplicationFactory applicationFactory)
        {
            this.Application = application;
            this.CustomTaskPanes = customTaskPanes;
            this.ApplicationFactory = applicationFactory;

            this.VstoWorksheetByInteropWorksheet = new Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Worksheet>();
            this.VstoWorkbookByInteropWorkbook = new Dictionary<Microsoft.Office.Interop.Excel.Workbook, Workbook>();
        }

        public Application Application { get; }

        public CustomTaskPaneCollection CustomTaskPanes { get; }

        public Worksheet ActiveWorksheet
        {
            get
            {
                var interop = this.Application.ActiveWorkbook.ActiveSheet;

                if (this.VstoWorksheetByInteropWorksheet.TryGetValue(interop, out Worksheet activeWorksheet))
                {
                    return activeWorksheet;
                }

                activeWorksheet = this.ApplicationFactory.GetVstoObject(interop);
                this.VstoWorksheetByInteropWorksheet[interop] = activeWorksheet;

                return activeWorksheet;
            }
        }

        public Workbook ActiveWorkbook
        {
            get
            {
                var interop = this.Application.ActiveWorkbook;

                if (this.VstoWorkbookByInteropWorkbook.TryGetValue(interop, out Workbook activeWorkbook))
                {
                    return activeWorkbook;
                }

                activeWorkbook = this.ApplicationFactory.GetVstoObject(interop);
                this.VstoWorkbookByInteropWorkbook[interop] = activeWorkbook;

                return activeWorkbook;
            }
        }

        private Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Worksheet> VstoWorksheetByInteropWorksheet { get; }

        private Dictionary<Microsoft.Office.Interop.Excel.Workbook, Workbook> VstoWorkbookByInteropWorkbook { get; }

        private ApplicationFactory ApplicationFactory { get; }

        public Worksheet GetVstoWorksheet(Microsoft.Office.Interop.Excel.Worksheet sheet)
        {
            this.VstoWorksheetByInteropWorksheet.TryGetValue(sheet, out var vstoWorksheet);
            return vstoWorksheet;
        }

        public ListObject GetVstoListObject(Microsoft.Office.Interop.Excel.ListObject interopListObject) => this.ApplicationFactory.GetVstoObject(interopListObject);

        public void EnsureSynchronizationContext()
        {
            if (SynchronizationContext.Current == null)
            {
                // ReSharper disable once UnusedVariable
                var form = new Form();
            }
        }
    }
}
