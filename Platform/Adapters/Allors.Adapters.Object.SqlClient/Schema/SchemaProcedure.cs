namespace Allors.Adapters.Object.SqlClient
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

        public string Name => this.name;

        public string Definition => this.definition;

        public override string ToString() => this.Name;

        public bool IsDefinitionCompatible(string existingDefinition) => this.RemoveWhitespace(this.Definition).Equals(this.RemoveWhitespace(existingDefinition));

        private string RemoveWhitespace(string input) => new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}
