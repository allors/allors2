// <copyright file="IChangeSetExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IChangeSetExtensions type.</summary>

namespace Allors.Database
{
    using System.Collections.Generic;
    using Collections;
    using Meta;

    public static partial class IChangeSetExtensions
    {
        private static readonly ISet<IRoleType> EmptyRoleTypeSet = new EmptySet<IRoleType>();

        private static readonly ISet<IAssociationType> EmptyAssociationTypeSet = new EmptySet<IAssociationType>();

        public static ISet<IRoleType> GetRoleTypes(this IChangeSet @this, IObject association) => @this.RoleTypesByAssociation.TryGetValue(association, out var roleTypes) ? roleTypes : EmptyRoleTypeSet;

        public static ISet<IAssociationType> GetAssociationTypes(this IChangeSet @this, IObject role) => @this.AssociationTypesByRole.TryGetValue(role, out var associationTypes) ? associationTypes : EmptyAssociationTypeSet;
    }
}
