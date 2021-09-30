// <copyright file="ObjectType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Schema
{
    using System;
    using System.Xml.Serialization;

    public partial class ObjectType
    {
        [XmlAttribute("i")]
        public Guid Id { get; set; }

        [XmlText]
        public string Objects { get; set; }
    }
}
