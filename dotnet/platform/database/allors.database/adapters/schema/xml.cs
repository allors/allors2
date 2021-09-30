// <copyright file="Xml.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
