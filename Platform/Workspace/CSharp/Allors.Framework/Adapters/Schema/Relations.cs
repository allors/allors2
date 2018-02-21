namespace Allors.Adapters.Schema
{
    using System.Xml.Serialization;

    public partial class Relations
    {
        [XmlArray("database")]
        [XmlArrayItem("rtc", typeof(RelationTypeComposite))]
        [XmlArrayItem("rtu", typeof(RelationTypeUnit))]
        public object[] RelationTypes;
    }
}
