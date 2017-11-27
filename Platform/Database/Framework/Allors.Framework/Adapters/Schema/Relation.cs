namespace Allors.Adapters.Schema
{
    using System.Xml.Serialization;

    public partial class Relation
    {
        [XmlAttribute("a")]
        public long Association;

        [XmlText]
        public string Role;
    }
}
