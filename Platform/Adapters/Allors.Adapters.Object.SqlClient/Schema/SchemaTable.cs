namespace Allors.Adapters.Object.SqlClient
{
    using System.Collections.Generic;

    public class SchemaTable
    {
        private readonly Dictionary<string, SchemaTableColumn> columnByLowercaseColumnName;

        public SchemaTable(Schema schema, string name)
        {
            this.Schema = schema;
            this.Name = name;

            this.columnByLowercaseColumnName = new Dictionary<string, SchemaTableColumn>();
        }

        public string Name { get; }

        public Dictionary<string, SchemaTableColumn> ColumnByLowercaseColumnName => this.columnByLowercaseColumnName;

        public Schema Schema { get; }

        public SchemaTableColumn GetColumn(string columnName)
        {
            SchemaTableColumn tableColumn;
            this.columnByLowercaseColumnName.TryGetValue(columnName.ToLowerInvariant(), out tableColumn);
            return tableColumn;
        }

        public override string ToString() => this.Name;
    }
}
