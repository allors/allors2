namespace Allors.Adapters.Object.Npgsql
{
    using System.Collections.Generic;

    public class SchemaTable
    {
        private readonly Schema schema;
        private readonly string name;
        private readonly Dictionary<string, SchemaTableColumn> columnByLowercaseColumnName;

        public SchemaTable(Schema schema, string name)
        {
            this.schema = schema;
            this.name = name;

            this.columnByLowercaseColumnName = new Dictionary<string, SchemaTableColumn>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Dictionary<string, SchemaTableColumn> ColumnByLowercaseColumnName
        {
            get
            {
                return this.columnByLowercaseColumnName;
            }
        }


        public Schema Schema
        {
            get
            {
                return this.schema;
            }
        }

        public SchemaTableColumn GetColumn(string columnName)
        {
            SchemaTableColumn tableColumn;
            this.columnByLowercaseColumnName.TryGetValue(columnName.ToLowerInvariant(), out tableColumn);
            return tableColumn;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
