// <copyright file="IControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Application.Excel
{
    using Allors.Excel;

    public interface IControl
    {

        ICell Cell { get; }

        void Bind();

        void Unbind();

        void OnCellChanged();
    }
}
