namespace Autotest.Html
{
    using Newtonsoft.Json.Linq;

    public class Attribute : Node
    {
        public Attribute(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string Name { get; set; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();
            this.Value = this.Json["value"]?.Value<string>();
        }
    }
}