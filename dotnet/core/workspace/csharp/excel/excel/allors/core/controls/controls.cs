// <copyright file="Controls.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Application.Excel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class Controls
    {
        public Controls(IWorksheet worksheet)
        {
            this.Worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
            this.Worksheet.CellsChanged += this.Worksheet_CellsChanged;
            this.ControlByCell = new ConcurrentDictionary<ICell, IControl>();
            this.ActiveControls = new HashSet<IControl>();
        }

        ~Controls()
        {
            this.Worksheet.CellsChanged -= this.Worksheet_CellsChanged;
        }

        public IWorksheet Worksheet { get; }

        public ConcurrentDictionary<ICell, IControl> ControlByCell { get; private set; }

        private HashSet<IControl> ActiveControls { get; }

        public string ExcelColumnFromNumber(int column)
        {
            string columnString = string.Empty;
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }

            return columnString;
        }

        public int ExcelColumnIndexFromName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.ToUpper(CultureInfo.CurrentCulture);
                int number = 0;
                int pow = 1;
                for (int i = name.Length - 1; i >= 0; i--)
                {
                    number += (name[i] - 'A' + 1) * pow;
                    pow *= 26;
                }

                return number;
            }

            return 0;
        }

        /// <summary>
        /// Shows a value of T in the provided cell.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        internal void Static<T>(int row, int column, T value, string numberFormat = null)
        {
            var cell = this.Worksheet[row, column];
            cell.NumberFormat = numberFormat;

            if (!this.ControlByCell.TryGetValue(cell, out var control))
            {
                control = new StaticContent<T>(cell);
                this.ControlByCell.TryAdd(cell, control);
            }

            //TODO: Koen, remove cast
            var staticContent = (StaticContent<T>)control;
            staticContent.Value = value;

            this.ActiveControls.Add(staticContent);
        }

        internal void Static<T>(Range range, T[] values)
        {
            // TODO: support vertical orientation
            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];
                var row = range.Row;
                var column = range.Column + i;
                this.Static(row, column, value);
            }
        }

        internal void Static<T>(Range range, T[][] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                var rowValues = values[i];
                for (var j = 0; j < rowValues.Length; j++)
                {
                    var value = rowValues[j];
                    var row = range.Row + i;
                    var column = range.Column + j;
                    this.Static(row, column, value);
                }
            }
        }

        /// <summary>
        /// Sets a Formula in the row, column. Formula is a string, starting with '='
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="formula"></param>
        internal void Formula(int row, int column, string formula)
        {
            var cell = this.Worksheet[row, column];

            if (!this.ControlByCell.TryGetValue(cell, out var control))
            {
                control = new FormulaControl(cell);
                this.ControlByCell.TryAdd(cell, control);
            }

            var formulaControl = (FormulaControl)control;
            formulaControl.Formula = formula;

            this.ActiveControls.Add(formulaControl);
        }

        internal void Hyperlink(int row, int column, string textToDisplay)
        {
            var cell = this.Worksheet[row, column];

            this.Worksheet.AddHyperLink(textToDisplay, cell);
           
        }

        /// <summary>
        /// Sets a readonly value in the cell. Changes are not handled.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="sessionObject"></param>
        /// <param name="roleType"></param>
        /// <param name="relationType"></param>
        internal void Label(int row, int column, ISessionObject sessionObject, RoleType roleType, RoleType relationType = null, string numberFormat = null)
        {
            if (sessionObject != null)
            {
                var cell = this.Worksheet[row, column];
                cell.NumberFormat = numberFormat;

                if (!this.ControlByCell.TryGetValue(cell, out var control))
                {
                    control = new Label(cell);
                    this.ControlByCell.TryAdd(cell, control);
                }

                var label = (Label)control;
                label.SessionObject = sessionObject;
                label.RoleType = roleType;
                label.RelationType = relationType;

                this.ActiveControls.Add(control);
            }
        }

        /// <inheritdoc cref="Application.Excel.TextBox"/>
        internal void TextBox(
            int row, int column, ISessionObject sessionObject, RoleType roleType, RoleType relationType = null,
            RoleType displayRoleType = null,
            string numberFormat = null,
            Func<object, dynamic> toDomain = null,
            Func<ISessionObject, dynamic> toCell = null,
            Func<ICell, ISessionObject> factory = null)
        {
            if (sessionObject != null || factory != null)
            {
                var cell = this.Worksheet[row, column];
                cell.NumberFormat = numberFormat;

                if (!this.ControlByCell.TryGetValue(cell, out var control))
                {
                    control = new TextBox(cell);
                    this.ControlByCell.TryAdd(cell, control);
                }

                var textBox = (TextBox)control;
                textBox.SessionObject = sessionObject;
                textBox.RoleType = roleType;
                textBox.RelationType = relationType;
                textBox.DisplayRoleType = displayRoleType;
                textBox.ToDomain = toDomain;
                textBox.ToCell = toCell;
                textBox.Factory = factory;

                this.ActiveControls.Add(control);
            }
        }

        internal ICell Select(int row, int column, Range options, ISessionObject sessionObject, RoleType roleType, RoleType relationType = null,
    RoleType displayRoleType = null,
    Func<object, dynamic> getRelation = null,
    string numberFormat = null,
    bool hideInCellDropDown = false)
        {
            ICell cell = null;
            if (sessionObject != null)
            {
                cell = this.Worksheet[row, column];
                cell.Options = options ?? throw new ArgumentNullException(nameof(options));
                cell.NumberFormat = numberFormat;
                cell.HideInCellDropdown = hideInCellDropDown;
                cell.IsRequired = roleType.IsRequired;

                if (!this.ControlByCell.TryGetValue(cell, out var control))
                {
                    control = new ComboBox(cell);
                    this.ControlByCell.TryAdd(cell, control);
                }

                var comboBox = (ComboBox)control;

                comboBox.SessionObject = sessionObject;
                comboBox.RoleType = roleType;
                comboBox.RelationType = relationType;
                comboBox.DisplayRoleType = displayRoleType;
                comboBox.ToDomain = getRelation;

                this.ActiveControls.Add(control);
            }

            return cell;
        }

        internal ICell Select(int row, int column, Range options, bool isRequired = false,
            RoleType displayRoleType = null,
            Func<object, dynamic> getRelation = null,
            string numberFormat = null,
            bool hideInCellDropDown = false)
        {
            var cell = this.Worksheet[row, column];
            cell.Options = options ?? throw new ArgumentNullException(nameof(options));
            cell.NumberFormat = numberFormat;
            cell.HideInCellDropdown = hideInCellDropDown;
            cell.IsRequired = isRequired;

            return cell;
        }

        internal CompositeControl Composite(int row, int column)
        {
            var cell = this.Worksheet[row, column];

            if (!this.ControlByCell.TryGetValue(cell, out var control))
            {
                control = new CompositeControl(this, cell);

                this.ControlByCell.TryAdd(cell, control);
            }

            this.ActiveControls.Add(control);

            return (CompositeControl)control;
        }

        internal void Bind()
        {
            var obsoleteControls = this.ControlByCell.Values.Except(this.ActiveControls);
            foreach (var control in this.ActiveControls)
            {
                control.Bind();
            }

            foreach (var control in obsoleteControls)
            {
                control.Unbind();
            }

            this.ControlByCell = new ConcurrentDictionary<ICell, IControl>(this.ActiveControls.ToDictionary(v => v.Cell));
        }

        private async void Worksheet_CellsChanged(object sender, CellChangedEvent e)
        {
            var changesReverted = false;

            foreach (var cell in e.Cells)
            {
                if (this.ControlByCell.TryGetValue(cell, out var control))
                {
                    control.OnCellChanged();

                    if (control is Label || IsGenericStaticContent(control))
                    {
                        changesReverted = true;
                    }
                }
            }

            if (changesReverted)
            {
                // a single message to the user should be done here:
            }

            await this.Worksheet.Flush();
        }

        private bool IsGenericStaticContent(IControl icontrol)
        {
            var type = icontrol.GetType();
            bool result1 = type.IsGenericType && type.FullName.Contains("StaticContent");

            return result1;
        }
    }
}
