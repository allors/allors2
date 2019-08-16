// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fetch.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System;
    using System.Linq;

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

        public Protocol.Data.Fetch ToJson()
        {
            return new Protocol.Data.Fetch
            {
                Step = this.Step?.ToJson(),
                Include = this.Include?.ToJson()
            };
        }
    }
}
