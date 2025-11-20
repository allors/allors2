// <copyright file="IChangeSetExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>

namespace Allors
{
    using System.Collections.Generic;

    using Database.Adapters;
    using Allors.Meta;

    public static partial class IChangeSetExtensions
    {
        private static readonly ISet<IRoleType> EmptyRoleTypeSet = new EmptySet<IRoleType>();

        private static readonly ISet<IAssociationType> EmptyAssociationTypeSet = new EmptySet<IAssociationType>();

        public static ISet<IRoleType> GetRoleTypes(this IChangeSet @this, long association) => @this.RoleTypesByAssociation.TryGetValue(association, out var roleTypes) ? roleTypes : EmptyRoleTypeSet;

        public static ISet<IAssociationType> GetAssociationTypes(this IChangeSet @this, long role) => @this.AssociationTypesByRole.TryGetValue(role, out var associationTypes) ? associationTypes : EmptyAssociationTypeSet;
    }
}
