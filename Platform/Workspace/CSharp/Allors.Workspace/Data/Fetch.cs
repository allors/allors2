// <copyright file="Fetch.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Linq;
    using Allors.Workspace.Meta;

    public class Fetch
    {
        public Fetch()
        {
        }

        public Fetch(params IPropertyType[] propertyTypes)
        {
            if (propertyTypes.Length > 0)
            {
                this.Step = new Step(propertyTypes, 0);
            }
        }

        public Fetch(IMetaPopulation metaPopulation, params Guid[] propertyTypeIds)
            : this(propertyTypeIds.Select(v => (IPropertyType)metaPopulation.Find(v)).ToArray())
        {
        }

        public Tree Include { get; set; }

        public Step Step { get; set; }

        public IObjectType ObjectType => this.Step?.GetObjectType() ?? this.Include.Composite;

        public Protocol.Data.Fetch ToJson() =>
            new Protocol.Data.Fetch
            {
                Step = this.Step?.ToJson(),
                Include = this.Include?.ToJson(),
            };
    }
}
