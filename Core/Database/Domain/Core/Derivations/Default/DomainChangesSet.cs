// <copyright file="IDomainChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Default
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using System.Collections;

    public class DomainChangeSet : IDomainChangeSet
    {
        public ISet<IObject> Created { get; }
        public ISet<IObject> Deleted { get; }
        public ISet<IObject> Associations { get; }
        public ISet<IObject> Roles { get; }
        public IDictionary<IObject, ISet<IRoleType>> RoleTypesByAssociation { get; }
        public IDictionary<IObject, ISet<IAssociationType>> AssociationTypesByRole { get; }
        public IDictionary<IRoleType, ISet<IObject>> AssociationsByRoleType { get; }
        public IDictionary<IAssociationType, ISet<IObject>> RolesByAssociationType { get; }

        internal DomainChangeSet(ISession session, IChangeSet changeSet)
        {
            this.Created = new HashSet<IObject>(session.Instantiate(changeSet.Created));
            this.Deleted = new HashSet<IObject>(session.Instantiate(changeSet.Deleted));
            this.Associations = new HashSet<IObject>(session.Instantiate(changeSet.Associations));
            this.Roles = new HashSet<IObject>(session.Instantiate(changeSet.Roles));
            this.RoleTypesByAssociation = changeSet.RoleTypesByAssociation.ToDictionary(v => session.Instantiate(v.Key), v => v.Value);
            this.AssociationTypesByRole = changeSet.AssociationTypesByRole.ToDictionary(v => session.Instantiate(v.Key), v => v.Value);

            this.AssociationsByRoleType = (from kvp in this.RoleTypesByAssociation
                                           from value in kvp.Value
                                           group kvp.Key by value)
                   .ToDictionary(grp => grp.Key, grp => new HashSet<IObject>(grp) as ISet<IObject> );

            this.RolesByAssociationType = (from kvp in this.AssociationTypesByRole
                                           from value in kvp.Value
                                           group kvp.Key by value)
                   .ToDictionary(grp => grp.Key, grp => new HashSet<IObject>(grp) as ISet<IObject>);
        }
    }
}
