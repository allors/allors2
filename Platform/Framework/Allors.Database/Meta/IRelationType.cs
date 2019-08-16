//------------------------------------------------------------------------------------------------- 
// <copyright file="IRelationType.cs" company="Allors bvba">
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
// <summary>Defines the RelationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;

    /// <summary>
    /// A relation type defines the state and behavior for
    /// a set of association types and role types.
    /// </summary>
    public interface IRelationType : IMetaObject, IComparable
    {
        IAssociationType AssociationType { get; }

        IRoleType RoleType { get; }

        Multiplicity Multiplicity { get; }

        bool ExistExclusiveClasses { get; }

        bool IsDerived { get; }

        bool IsSynced { get; }

        bool IsIndexed { get; }
    }
}
