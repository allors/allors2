// <copyright file="Relations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Schema
{
    using System.Xml.Serialization;

    public partial class Relations
    {
        [XmlArray("database")]
        [XmlArrayItem("rtc", typeof(RelationTypeComposite))]
        [XmlArrayItem("rtu", typeof(RelationTypeUnit))]
        public object[] RelationTypes { get; set; }
    }
}
