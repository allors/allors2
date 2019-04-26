namespace Autotest.Html
{
    using Newtonsoft.Json.Linq;

    public class Text : Node
    {
        public Text(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Value = this.Json["value"]?.Value<string>();
        }
    }
}