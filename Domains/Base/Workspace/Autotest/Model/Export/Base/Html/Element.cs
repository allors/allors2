namespace Autotest.Html
{
    public class Element : Node
    {
        public string Name { get; set; }

        public Node[] Children { get; set; }

        public Attribute[] Attributes { get; set; }

    }
}