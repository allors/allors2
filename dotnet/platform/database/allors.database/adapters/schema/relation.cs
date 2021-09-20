// <copyright file="Relation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Schema
{
    using System.Xml.Serialization;

    public partial class Relation
    {
        [XmlAttribute("a")]
        public long Association { get; set; }

        [XmlText]
        public string Role { get; set; }
    }
}
