namespace Autotest.Html
{
    using Newtonsoft.Json.Linq;

    public class Comment : Node
    {
        public Comment(JToken json)
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