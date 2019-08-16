namespace Allors.Adapters.Object.SqlClient
{
    using System.Collections.Generic;

    public class SchemaTableType
    {
        private readonly Dictionary<string, SchemaTableTypeColumn> columnByLowercaseColumnName;

        public SchemaTableType(Schema schema, string name)
        {
            this.Name = name;
            this.columnByLowercaseColumnName = new Dictionary<string, SchemaTableTypeColumn>();
        }

        public string Name { get; }

        public Dictionary<string, SchemaTableTypeColumn> ColumnByLowercaseColumnName => this.columnByLowercaseColumnName;

        public override string ToString() => this.Name;

        public SchemaTableTypeColumn GetColumn(string columnName)
        {
            SchemaTableTypeColumn tableColumn;
            this.columnByLowercaseColumnName.TryGetValue(columnName.ToLowerInvariant(), out tableColumn);
            return tableColumn;
        }
    }
}
