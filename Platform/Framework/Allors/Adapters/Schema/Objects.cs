namespace Allors.Adapters.Schema
{
    using System.Xml.Serialization;

    public partial class Objects
    {
        [XmlArray("database")]
        [XmlArrayItem("ot")]
        public ObjectType[] ObjectTypes { get; set; }
    }
}
