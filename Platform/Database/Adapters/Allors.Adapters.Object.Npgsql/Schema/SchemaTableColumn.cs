namespace Allors.Adapters.Object.Npgsql
{
    public class SchemaTableColumn
    {
        public SchemaTableColumn(SchemaTable table, string name, string dataType, int? characterMaximumLength, int? numericPrecision, int? numericScale)
        {
            this.Table = table;
            this.Name = name;
            this.LowercaseName = name.ToLowerInvariant();
            this.DataType = dataType;
            this.CharacterMaximumLength = characterMaximumLength;
            this.NumericPrecision = numericPrecision;
            this.NumericScale = numericScale;
        }

        public SchemaTable Table { get; }

        public string Name { get; }

        public string LowercaseName { get; }

        public string DataType { get; }

        public string SqlType
        {
            get
            {
                switch (this.DataType)
                {
                    case "varchar":
                    case "character varying":
                        return $"varchar({this.CharacterMaximumLength})";

                    case "int4":
                        return "integer";

                    case "numeric":
                        return "numeric(" + this.NumericPrecision + "," + this.NumericScale + ")";

                    case "timestamp without time zone":
                        return "timestamp";

                    default:
                        return this.DataType;
                }
            }
        }

        public int? CharacterMaximumLength { get; }

        public int? NumericPrecision { get; }

        public int? NumericScale { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}