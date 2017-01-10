namespace Allors.Adapters.Relation.SqlClient
{
    public class TableColumn
    {
        private readonly Table table;
        private readonly string name;
        private readonly string lowercaseName;
        private readonly string dataType;
        private readonly int? characterMaximumLength;
        private readonly int? numericPrecision;
        private readonly int? numericScale;

        public TableColumn(Table table, string name, string dataType, int? characterMaximumLength, int? numericPrecision, int? numericScale)
        {
            this.table = table;
            this.name = name;
            this.lowercaseName = name.ToLowerInvariant();
            this.dataType = dataType;
            this.characterMaximumLength = characterMaximumLength;
            this.numericPrecision = numericPrecision;
            this.numericScale = numericScale;
        }

        public Table Table
        {
            get
            {
                return this.table;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string LowercaseName
        {
            get
            {
                return this.lowercaseName;
            }
        }

        public string DataType
        {
            get
            {
                return this.dataType;
            }
        }

        public string SqlType
        {
            get
            {
                if (this.dataType.Equals("nvarchar"))
                {
                    var length = this.CharacterMaximumLength == -1 ? "MAX" : this.CharacterMaximumLength.ToString();
                    return "nvarchar(" + length + ")";
                }

                if (this.dataType.Equals("varbinary"))
                {
                    var length = this.CharacterMaximumLength == -1 ? "MAX" : this.CharacterMaximumLength.ToString();
                    return "varbinary(" + length + ")";
                }


                if (this.dataType.Equals("decimal"))
                {
                    return "decimal(" + this.numericPrecision + "," + this.numericScale + ")";
                }

                return this.dataType;
            }
        }

        public int? CharacterMaximumLength
        {
            get
            {
                return this.characterMaximumLength;
            }
        }

        public int? NumericPrecision
        {
            get
            {
                return this.numericPrecision;
            }
        }

        public int? NumericScale
        {
            get
            {
                return this.numericScale;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}