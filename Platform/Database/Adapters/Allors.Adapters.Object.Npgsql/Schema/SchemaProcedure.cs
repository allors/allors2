namespace Allors.Adapters.Object.Npgsql
{
    using System.Linq;

    public class SchemaProcedure
    {
        private readonly string name;
        private readonly string definition;

        public SchemaProcedure(Schema schema, string name, string definition)
        {
            this.name = name;
            this.definition = definition;
        }

        public string Name
        {
            get
            {
                return this.name;
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