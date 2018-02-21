namespace Allors.Adapters.Schema
{
    using System;
    using System.Xml.Serialization;

    public partial class ObjectType
    {
        [XmlAttribute("i")]
        public Guid Id;
        
        [XmlText]
        public string Objects;
    }
}
