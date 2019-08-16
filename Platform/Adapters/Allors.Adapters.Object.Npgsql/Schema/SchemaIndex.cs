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

        public string Name => this.name;

        public string LowercaseName => this.lowercaseName;

        public Schema Schema => this.schema;

        public override string ToString() => this.Name;
    }
}
