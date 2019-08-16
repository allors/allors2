namespace Allors.Workspace.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Microsoft.Office.Interop.Excel;

    using DataTable = System.Data.DataTable;
    using ListObject = Microsoft.Office.Tools.Excel.ListObject;

    public abstract partial class Table<T> : DataTable
        where T : DataRow
    {
        public T this[int idx] => (T)this.Rows[idx];

        public void Add(T row) => this.Rows.Add(row);

        public void Remove(T row) => this.Rows.Remove(row);

        public new T NewRow() => (T)base.NewRow();

        public void FromListObject(ListObject listObject)
        {
            var columnByIndex = new Dictionary<int, DataColumn>();
            for (var index = 0; index < this.Columns.Count; index++)
            {
                var column = this.Columns[index];
                columnByIndex.Add(index, column);
            }
            
            var listRows = listObject
                .ListRows
                .Cast<ListRow>()
                .Select(v => v.Range.Cells.Cast<Range>().Select(cell => cell.Value).ToArray())
                .ToArray();

            foreach (var listRow in listRows)
            {
                var row = this.NewRow();

                foreach (var kvp in columnByIndex)
                {
                    var index = kvp.Key;
                    var column = kvp.Value;

                    row[column] = listRow[index] ?? DBNull.Value;
                }

                this.Add(row);
            }
        }

        public void ToListObject(ListObject listObject)
        {
            var dataSet = new DataSet();
            dataSet.Tables.Add(this);
            listObject.SetDataBinding(dataSet, this.TableName);

            var headers = listObject.HeaderRowRange;
            for (var i = 0; i < this.Columns.Count; i++)
            {
                var column = this.Columns[i];
                Range header = headers.Cells[1, i + 1];
                header.Value = column.ColumnName;
            }
        }

        protected override Type GetRowType() => typeof(T);

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder) => (T)Activator.CreateInstance(typeof(T), builder);
    }
}