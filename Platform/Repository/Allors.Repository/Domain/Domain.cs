// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Domain
    {
        internal Domain(Guid id, DirectoryInfo directoryInfo)
        {
            this.Id = id;
            this.DirectoryInfo = directoryInfo;

            this.PartialInterfaceByName = new Dictionary<string, PartialInterface>();
            this.PartialClassBySingularName = new Dictionary<string, PartialClass>();
            this.PartialTypeBySingularName = new Dictionary<string, PartialType>();
        }

        public Guid Id { get; set; }

        public DirectoryInfo DirectoryInfo { get; set; }

        public string Name => this.DirectoryInfo.Name;

        public Domain Base { get; set; }

        public Dictionary<string, PartialInterface> PartialInterfaceByName { get; }

        public Dictionary<string, PartialClass> PartialClassBySingularName { get; }

        public Dictionary<string, PartialType> PartialTypeBySingularName { get; }

        public override string ToString() => this.Name;
    }
}
