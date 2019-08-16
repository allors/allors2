//------------------------------------------------------------------------------------------------- 
// <copyright file="IAssociationType.cs" company="Allors bvba">
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
// <summary>Defines the AssociationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    /// <summary>
    /// An association type defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public interface IAssociationType : IPropertyType
    {
        string SingularName { get; }

        string SingularPropertyName { get; }

        string SingularFullName { get; }

        string PluralName { get; }

        string PluralPropertyName { get; }

        string PluralFullName { get; }

        bool IsMany { get; }

        IRelationType RelationType { get; }

        IComposite ObjectType { get; }

        bool IsOne { get; }

        IRoleType RoleType { get; }
    }
}
