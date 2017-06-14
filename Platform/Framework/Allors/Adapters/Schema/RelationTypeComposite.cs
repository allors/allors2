namespace Allors.Adapters.Schema
{
    using System;
    using System.Xml.Serialization;

    public partial class RelationTypeComposite
    {
        [XmlAttribute("i")]
        public Guid Id { get; set; }

        [XmlElement("r")]
        public Relation[] Relations { get; set; }
    }
}
