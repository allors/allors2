namespace Allors.Adapters.Object.SqlClient
{
    public class SchemaTableColumn
    {
        private readonly string dataType;
        private readonly int? numericScale;

        public SchemaTableColumn(SchemaTable table, string name, string dataType, int? characterMaximumLength, int? numericPrecision, int? numericScale)
        {
            this.Table = table;
            this.Name = name;
            this.LowercaseName = name.ToLowerInvariant();
            this.dataType = dataType;
            this.CharacterMaximumLength = characterMaximumLength;
            this.NumericPrecision = numericPrecision;
            this.numericScale = numericScale;
        }

        public SchemaTable Table { get; }

        public string Name { get; }

        public string LowercaseName { get; }

        public string DataType => this.dataType;

        public string SqlType
        {
            get
            {
                if (this.dataType.Equals("nvarchar"))
                {
                    var length = this.CharacterMaximumLength == -1 ? "max" : this.CharacterMaximumLength.ToString();
                    return "nvarchar(" + length + ")";
                }

                if (this.dataType.Equals("varbinary"))
                {
                    var length = this.CharacterMaximumLength == -1 ? "max" : this.CharacterMaximumLength.ToString();
                    return "varbinary(" + length + ")";
                }

                if (this.dataType.Equals("decimal"))
                {
                    return "decimal(" + this.NumericPrecision + "," + this.numericScale + ")";
                }

                return this.dataType;
            }
        }

        public int? CharacterMaximumLength { get; }

        public int? NumericPrecision { get; }

        public int? NumericScale => this.numericScale;

        public override string ToString() => this.Name;
    }
}
