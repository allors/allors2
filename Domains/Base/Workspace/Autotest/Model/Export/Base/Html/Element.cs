namespace Autotest.Html
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Element : Node
    {
        public Element(JToken json)
        {
            this.Json = json;
        }

        public JToken Json { get; }

        public string Name { get; set; }

        public Node[] Children { get; set; }

        public Attribute[] Attributes { get; set; }

        public void BaseLoad()
        {
            this.Name = this.Json["name"]?.Value<string>();

            var jsonChildren = this.Json["children"];
            this.Children = jsonChildren != null ? jsonChildren.Select(v =>
                {
                    var node = NodeFactory.Create(v);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new Node[0];

            var jsonAttributes = this.Json["attributes"];
            this.Attributes = jsonAttributes != null ? jsonAttributes.Select(v =>
                {
                    var attribute = new Attribute(v);
                    attribute.BaseLoad();
                    return attribute;
                }).ToArray() : new Attribute[0];
        }
    }
}