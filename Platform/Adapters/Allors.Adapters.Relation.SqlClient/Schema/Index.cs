namespace Allors.Adapters.Relation.SqlClient
{
    using System.Collections.Generic;
    using System.Linq;

    public class Index
    {
        private readonly Schema schema;
        private readonly string name;
        private readonly string lowercaseName;

        public Index(Schema schema, string name)
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

        public Table Table { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}