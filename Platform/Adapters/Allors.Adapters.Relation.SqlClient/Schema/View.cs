namespace Allors.Adapters.Relation.SqlClient
{
    using System.Collections.Generic;
    using System.Linq;

    public class View
    {
        private readonly Schema schema;
        private readonly string name;
        private readonly string lowercaseName;
        private readonly string definition;
        private readonly HashSet<Table> tables;

        public View(Schema schema, string name, string definition)
        {
            this.schema = schema;
            this.name = name;
            this.definition = definition;
            this.lowercaseName = name.ToLowerInvariant();

            this.tables = new HashSet<Table>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public HashSet<Table> Tables
        {
            get
            {
                return this.tables;
            }
        }

        public string LowercaseName
        {
            get
            {
                return this.lowercaseName;
            }
        }

        public string Definition
        {
            get
            {
                return this.definition;
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

        public bool IsDefinitionCompatible(string existingDefinition)
        {
            return this.RemoveWhitespace(this.Definition).Equals(this.RemoveWhitespace(existingDefinition));
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}