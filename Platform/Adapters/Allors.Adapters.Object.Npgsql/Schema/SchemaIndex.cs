namespace Allors.Adapters.Object.Npgsql
{
    public class SchemaIndex
    {
        private readonly Schema schema;
        private readonly string name;
        private readonly string lowercaseName;

        public SchemaIndex(Schema schema, string name)
        {
            this.schema = schema;
            this.name = name;
            this.lowercaseName = name.ToLowerInvariant();
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

        public Schema Schema
        {
            get
            {
                return this.schema;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
