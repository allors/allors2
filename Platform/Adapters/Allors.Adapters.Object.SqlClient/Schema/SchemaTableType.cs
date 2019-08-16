namespace Allors.Adapters.Object.SqlClient
{
    using System.Collections.Generic;

    public class SchemaTableType
    {
        private readonly string name;
        private readonly Dictionary<string, SchemaTableTypeColumn> columnByLowercaseColumnName;

        public SchemaTableType(Schema schema, string name)
        {
            this.name = name;
            this.columnByLowercaseColumnName = new Dictionary<string, SchemaTableTypeColumn>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Dictionary<string, SchemaTableTypeColumn> ColumnByLowercaseColumnName
        {
            get
            {
                return this.columnByLowercaseColumnName;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public SchemaTableTypeColumn GetColumn(string columnName)
        {
            SchemaTableTypeColumn tableColumn;
            this.columnByLowercaseColumnName.TryGetValue(columnName.ToLowerInvariant(), out tableColumn);
            return tableColumn;
        }
    }
}
