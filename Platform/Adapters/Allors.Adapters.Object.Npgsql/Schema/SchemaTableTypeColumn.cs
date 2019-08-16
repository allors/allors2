namespace Allors.Adapters.Object.Npgsql
{
    public class SchemaTableTypeColumn
    {
        private readonly string dataType;
        private readonly int? scale;

        public SchemaTableTypeColumn(SchemaTableType table, string name, string dataType, int? maximumLength, int? precision, int? scale)
        {
            this.Table = table;
            this.Name = name;
            this.dataType = dataType;
            this.MaximumLength = maximumLength;
            this.Precision = precision;
            this.scale = scale;
        }

        public SchemaTableType Table { get; }

        public string Name { get; }

        public string DataType => this.dataType;

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
                    return "decimal(" + this.Precision + "," + this.scale + ")";
                }

                return this.dataType;
            }
        }

        public int? MaximumLength { get; }

        public int? Precision { get; }

        public int? Scale => this.scale;

        public override string ToString() => this.Name;
    }
}
