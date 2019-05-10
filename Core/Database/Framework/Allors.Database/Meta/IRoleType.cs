//------------------------------------------------------------------------------------------------- 
// <copyright file="IRoleType.cs" company="Allors bvba">
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
// <summary>Defines the RoleType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    /// <summary>
    /// A <see cref="IRoleType"/> defines the role side of a relation.
    /// This is also called the 'passive' side.
    /// RoleTypes can have composite and unit <see cref="ObjectType"/>s.
    /// </summary>
    public interface IRoleType : IPropertyType
    {
        string SingularName { get; }

        string SingularPropertyName { get; }

        string SingularFullName { get; }

        string PluralName { get; }

        string PluralPropertyName { get; }

        string PluralFullName { get; }

        IObjectType ObjectType { get; }

        IAssociationType AssociationType { get; }

        IRelationType RelationType { get; }

        bool IsMany { get; }

        bool IsOne { get; }

        int? Size { get; }

        int? Precision { get; }

        int? Scale { get; }
    }
}