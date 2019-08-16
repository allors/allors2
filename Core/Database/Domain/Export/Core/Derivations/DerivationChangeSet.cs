
// <copyright file="DerivationChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    public class AccumulatedChangeSet : IChangeSet
    {
        internal AccumulatedChangeSet()
        {
            this.Created = new HashSet<long>();
            this.Deleted = new HashSet<long>();
            this.Associations = new HashSet<long>();
            this.Roles = new HashSet<long>();
            this.RoleTypesByAssociation = new Dictionary<long, ISet<IRoleType>>();
            this.AssociationTypesByRole = new Dictionary<long, ISet<IAssociationType>>();
        }

        public ISet<long> Created { get; }

        public ISet<long> Deleted { get; }

        public ISet<long> Associations { get; }

        public ISet<long> Roles { get; }

        public IDictionary<long, ISet<IRoleType>> RoleTypesByAssociation { get; }

        public IDictionary<long, ISet<IAssociationType>> AssociationTypesByRole { get; }

        public void Add(IChangeSet changeSet)
        {
            this.Created.UnionWith(changeSet.Created);
            this.Deleted.UnionWith(changeSet.Deleted);
            this.Associations.UnionWith(changeSet.Associations);
            this.Roles.UnionWith(changeSet.Roles);

            foreach (var kvp in changeSet.RoleTypesByAssociation)
            {
                if (this.RoleTypesByAssociation.TryGetValue(kvp.Key, out var roleTypes))
                {
                    roleTypes.UnionWith(kvp.Value);
                }
                else
                {
                    this.RoleTypesByAssociation[kvp.Key] = new HashSet<IRoleType>(changeSet.RoleTypesByAssociation[kvp.Key]);
                }
            }

            foreach (var kvp in changeSet.AssociationTypesByRole)
            {
                if (this.AssociationTypesByRole.TryGetValue(kvp.Key, out var associationTypes))
                {
                    associationTypes.UnionWith(kvp.Value);
                }
                else
                {
                    this.AssociationTypesByRole[kvp.Key] = new HashSet<IAssociationType>(changeSet.AssociationTypesByRole[kvp.Key]);
                }
            }
        }
    }
}
