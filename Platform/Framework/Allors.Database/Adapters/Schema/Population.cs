namespace Allors.Adapters.Schema
{
    using System.Xml.Serialization;

    public partial class Population
    {
        [XmlAttribute("version")]
        public int Version;

        [XmlElement("objects")]
        public Objects Objects;

        [XmlElement("relations")]
        public Relations Relations;
    }
}
