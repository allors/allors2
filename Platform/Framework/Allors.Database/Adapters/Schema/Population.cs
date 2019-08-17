// <copyright file="Population.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
