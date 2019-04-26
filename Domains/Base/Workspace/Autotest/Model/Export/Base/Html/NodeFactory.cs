namespace Autotest.Html
{
    using System;

    using Newtonsoft.Json.Linq;

    public static class NodeFactory
    {
        public static Node Create(JToken json)
        {
            var kind = json["kind"]?.Value<string>();
            switch (kind)
            {
                case "element":
                    return new Element(json);
                case "text":
                    return new Text(json);
                case "comment":
                    return new Comment(json);
                case "expansion":
                    return new Expansion(json);
                default:
                    throw new Exception($"Unknown kind: {kind}");
            }
        }
    }
}