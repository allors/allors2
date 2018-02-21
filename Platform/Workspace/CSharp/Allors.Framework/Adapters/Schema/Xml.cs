namespace Allors.Adapters.Schema
{
    using System.Xml.Serialization;
    [XmlRoot("allors")]
    public partial class Xml
    {
        [XmlElement("population")]
        public Population Population;
    }
}
