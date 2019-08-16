//-------------------------------------------------------------------------------------------------
// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System.Collections.Generic;

    using Allors.Adapters;
    using Allors.Meta;

    public static partial class IChangeSetExtensions
    {
        private static readonly ISet<IRoleType> EmptyRoleTypeSet = new EmptySet<IRoleType>();

        private static readonly ISet<IAssociationType> EmptyAssociationTypeSet = new EmptySet<IAssociationType>();

        public static ISet<IRoleType> GetRoleTypes(this IChangeSet @this, long association)
        {
            return @this.RoleTypesByAssociation.TryGetValue(association, out var roleTypes) ? roleTypes : EmptyRoleTypeSet;
        }

        public static ISet<IAssociationType> GetAssociationTypes(this IChangeSet @this, long role)
        {
            return @this.AssociationTypesByRole.TryGetValue(role, out var associationTypes) ? associationTypes : EmptyAssociationTypeSet;
        }
    }
}
