using Autotest.Html;

namespace Autotest.Angular
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Template
    {
        public Directive Directive { get; }

        public JToken Json { get; }

        private string Url { get; set; }

        Node[] Html { get; set; }
        
        public Template(Directive directive, JToken json)
        {
            this.Directive = directive;
            this.Json = json;
        }

        public void BaseLoad()
        {
            this.Url = this.Json["url"]?.Value<string>();

            var jsonHtml = this.Json["html"];
            this.Html = jsonHtml != null ? jsonHtml.Select(v =>
                {
                    var node = NodeFactory.Create(v);
                    node.BaseLoad();
                    return node;
                }).ToArray() : new Node[0];

        }
    }
}