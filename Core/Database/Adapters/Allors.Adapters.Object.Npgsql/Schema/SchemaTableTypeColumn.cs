namespace Allors.Adapters.Object.Npgsql
{
    public class SchemaTableTypeColumn
    {
        private readonly SchemaTableType table;
        private readonly string name;
        private readonly string dataType;
        private readonly int? maximumLength;
        private readonly int? precision;
        private readonly int? scale;

        public SchemaTableTypeColumn(SchemaTableType table, string name, string dataType, int? maximumLength, int? precision, int? scale)
        {
            this.table = table;
            this.name = name;
            this.dataType = dataType;
            this.maximumLength = maximumLength;
            this.precision = precision;
            this.scale = scale;
        }

        public SchemaTableType Table
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
                    var length = this.MaximumLength == -1 ? "max" : this.MaximumLength.ToString();
                    return "nvarchar(" + length + ")";
                }

                if (this.dataType.Equals("varbinary"))
                {
                    var length = this.MaximumLength == -1 ? "max" : this.MaximumLength.ToString();
                    return "varbinary(" + length + ")";
                }


                if (this.dataType.Equals("decimal"))
                {
                    return "decimal(" + this.precision + "," + this.scale + ")";
                }

                return this.dataType;
            }
        }

        public int? MaximumLength
        {
            get
            {
                return this.maximumLength;
            }
        }

        public int? Precision
        {
            get
            {
                return this.precision;
            }
        }

        public int? Scale
        {
            get
            {
                return this.scale;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}