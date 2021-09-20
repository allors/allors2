// <copyright file="ChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    internal sealed class ChangeSet : IChangeSet
    {
        private IDictionary<IRoleType, ISet<IObject>> associationsByRoleType;
        private IDictionary<IAssociationType, ISet<IObject>> rolesByAssociationType;

        private ISet<IObject> associations;
        private ISet<IObject> roles;

        internal ChangeSet(ISet<IObject> created, ISet<IStrategy> deleted, IDictionary<IObject, ISet<IRoleType>> roleTypesByAssociation, IDictionary<IObject, ISet<IAssociationType>> associationTypesByRole)
        {
            this.Created = created;
            this.Deleted = deleted;
            this.RoleTypesByAssociation = roleTypesByAssociation;
            this.AssociationTypesByRole = associationTypesByRole;
        }

        public ISet<IObject> Created { get; }

        public ISet<IStrategy> Deleted { get; }

        public IDictionary<IObject, ISet<IRoleType>> RoleTypesByAssociation { get; }

        public IDictionary<IObject, ISet<IAssociationType>> AssociationTypesByRole { get; }

        public ISet<IObject> Associations => this.associations ??= new HashSet<IObject>(this.RoleTypesByAssociation.Keys);

        public ISet<IObject> Roles => this.roles ??= new HashSet<IObject>(this.AssociationTypesByRole.Keys);

        public IDictionary<IRoleType, ISet<IObject>> AssociationsByRoleType => this.associationsByRoleType ??=
            (from kvp in this.RoleTypesByAssociation
             from value in kvp.Value
             group kvp.Key by value)
                 .ToDictionary(grp => grp.Key, grp => new HashSet<IObject>(grp) as ISet<IObject>);

        public IDictionary<IAssociationType, ISet<IObject>> RolesByAssociationType => this.rolesByAssociationType ??=
            (from kvp in this.AssociationTypesByRole
             from value in kvp.Value
             group kvp.Key by value)
                   .ToDictionary(grp => grp.Key, grp => new HashSet<IObject>(grp) as ISet<IObject>);
    }
}
