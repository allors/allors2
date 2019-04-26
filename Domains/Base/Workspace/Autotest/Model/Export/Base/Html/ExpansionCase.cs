namespace Autotest.Html
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class ExpansionCase : Node
    {
     
        public ExpansionCase(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public Node[] Expression { get; set; }

        public string Value { get; set; }

        public void BaseLoad()
        {
            this.Value = this.Json["value"]?.Value<string>();

            var expressionChildren = this.Json["expression"];
            this.Expression = expressionChildren != null ? expressionChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new Node[0];
        }
    }
}