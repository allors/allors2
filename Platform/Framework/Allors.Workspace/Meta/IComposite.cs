//------------------------------------------------------------------------------------------------- 
// <copyright file="IComposite.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;

    public interface IComposite : IObjectType
    {
        IEnumerable<IAssociationType> AssociationTypes { get; }

        IEnumerable<IRoleType> RoleTypes { get; }

        IEnumerable<IInterface> DirectSupertypes { get; }

        IEnumerable<IInterface> Supertypes { get; }

        bool ExistSupertype(IInterface @interface);

        bool ExistAssociationType(IAssociationType association);

        bool ExistRoleType(IRoleType roleType);

        bool IsAssignableFrom(IComposite objectType);

        bool ExistClass { get; }

        IEnumerable<IClass> Classes { get; }

        bool ExistExclusiveClass { get; }

        IClass ExclusiveClass { get; }

        bool IsSynced { get; }

        bool AssignedIsSynced { get; set; }
    }
}
