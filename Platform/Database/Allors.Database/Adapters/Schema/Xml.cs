// <copyright file="Xml.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Schema
{
    using System.Xml.Serialization;

    [XmlRoot("allors")]
    public partial class Xml
    {
        [XmlElement("population")]
        public Population Population { get; set; }
    }
}
