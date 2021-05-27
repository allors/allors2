// <copyright file="Label.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Application.Excel
{
    using Allors.Excel;

    public class FormulaControl : IControl
    {
        public FormulaControl(ICell cell)
        {
            this.Cell = cell;
        }       

        public ICell Cell { get; set; }

        public string Formula { get; set; }

        public void Bind()
        {
            this.Cell.Formula = this.Formula;
        }

        public void OnCellChanged()
        {
           // TODO throw exception, or revert Changes?
        }

        public void Unbind()
        {
            // TODO
        }      
    }
}
