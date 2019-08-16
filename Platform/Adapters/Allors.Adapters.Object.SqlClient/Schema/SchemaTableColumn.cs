namespace Allors.Adapters.Object.SqlClient
{
    public class SchemaTableColumn
    {
        private readonly SchemaTable table;
        private readonly string name;
        private readonly string lowercaseName;
        private readonly string dataType;
        private readonly int? characterMaximumLength;
        private readonly int? numericPrecision;
        private readonly int? numericScale;

        public SchemaTableColumn(SchemaTable table, string name, string dataType, int? characterMaximumLength, int? numericPrecision, int? numericScale)
        {
            this.table = table;
            this.name = name;
            this.lowercaseName = name.ToLowerInvariant();
            this.dataType = dataType;
            this.characterMaximumLength = characterMaximumLength;
            this.numericPrecision = numericPrecision;
            this.numericScale = numericScale;
        }

        public SchemaTable Table => this.table;

        public string Name => this.name;

        public string LowercaseName => this.lowercaseName;

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
                    return "decimal(" + this.numericPrecision + "," + this.numericScale + ")";
                }

                return this.dataType;
            }
        }

        public int? CharacterMaximumLength => this.characterMaximumLength;

        public int? NumericPrecision => this.numericPrecision;

        public int? NumericScale => this.numericScale;

        public override string ToString() => this.Name;
    }
}
