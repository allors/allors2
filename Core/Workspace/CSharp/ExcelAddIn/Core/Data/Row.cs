// <copyright file="Row.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Domain
{
    using System;
    using System.Data;

    public abstract partial class Row<T> : DataRow
        where T : DataTable
    {
        public new readonly T Table;

        public Row(DataRowBuilder builder)
            : base(builder) =>
            this.Table = (T)base.Table;

        public new object this[DataColumn column]
        {
            get
            {
                var item = base[column];
                return item == DBNull.Value ? null : item;
            }

            set => base[column] = value ?? DBNull.Value;
        }
    }
}