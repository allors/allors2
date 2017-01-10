namespace Allors.Adapters.Relation.SqlClient
{
    using System.Linq;

    public class Procedure
    {
        private readonly Schema schema;
        private readonly string name;
        private readonly string lowercaseName;
        private readonly string definition;

        public Procedure(Schema schema, string name, string definition)
        {
            this.schema = schema;
            this.name = name;
            this.lowercaseName = name.ToLowerInvariant();
            this.definition = definition;
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

        public string Definition
        {
            get
            {
                return this.definition;
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