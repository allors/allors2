//------------------------------------------------------------------------------------------------- 
// <copyright file="CompositePredicateAssertions.cs" company="Allors bvba">
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
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    /// <summary>
    /// Utility class for <see cref="ICompositePredicate"/>s containing several assertions.
    /// </summary>
    public class PredicateAssertions
    {
        /// <summary>
        /// Asserts that the <see cref="AssociationType"/> and the <see cref="Extent"/> are compatible with
        /// <see cref="ICompositePredicate#AddContainedIn"/>.
        /// </summary>
        /// <param name="association">The association.</param>
        /// <param name="extent">The extent.</param>
        public static void AssertAssociationContainedIn(IAssociationType association, Extent extent)
        {
            // TODO: ?
        }

        public static void AssertAssociationContainedIn(IAssociationType association, IEnumerable<IObject> enumerable)
        {
            // TODO: ?
        }

        /// <summary>
        /// Asserts that the <see cref="AssociationType"/> and the <see cref="IObject"/> are compatible with
        /// <see cref="ICompositePredicate#AddContains"/>.
        /// </summary>
        /// <param name="association">The association.</param>
        /// <param name="allorsObject">The allors object.</param>
        public static void AssertAssociationContains(IAssociationType association, IObject allorsObject)
        {
            if (!association.IsMany)
            {
                throw new ArgumentException(
                    "AddContains() can only be used when the association has multiplicity many, use AssociationEquals() instead.");
            }

            if (allorsObject == null)
            {
                throw new ArgumentException("AddContains() requires an object.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="AssociationType"/> and the <see cref="IObject"/> are compatible with
        /// <see cref="ICompositePredicate#AddEquals"/>.
        /// </summary>
        /// <param name="association">The association.</param>
        /// <param name="allorsObject">The allors object.</param>
        public static void AssertAssociationEquals(IAssociationType association, IObject allorsObject)
        {
            if (!association.IsOne)
            {
                throw new ArgumentException(
                    "AddEquals() can only be used when association has multiplicity one, use AssociationContains() instead.");
            }

            if (allorsObject == null)
            {
                throw new ArgumentException(
                    "AddEquals() requires a non-null contained object, use AddNot().AddExists() instead.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="AssociationType"/> is compatible with
        /// <see cref="ICompositePredicate#AddExists"/>.
        /// </summary>
        /// <param name="association">The association.</param>
        public static void ValidateAssociationExists(IAssociationType association)
        {
            // TODO: ?
        }

        /// <summary>
        /// Asserts that the <see cref="AssociationType"/> and the <see cref="IObjectType"/> are compatible with
        /// <see cref="ICompositePredicate#AddInstanceof"/>.
        /// </summary>
        /// <param name="association">The association.</param>
        /// <param name="objectType">The object type.</param>
        public static void ValidateAssociationInstanceof(IAssociationType association, IObjectType objectType)
        {
            if (objectType is IUnit)
            {
                throw new ArgumentException("AddInstanceof() can only be used with a composite type.");
            }

            if (association.IsMany)
            {
                throw new ArgumentException(
                    "AddInstanceof() can only be used when the association has multiplicity one.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="IObjectType"/> is compatible with
        /// <see cref="ICompositePredicate#AddInstanceof"/>.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        public static void ValidateInstanceof(IObjectType objectType)
        {
            if (objectType is IUnit)
            {
                throw new ArgumentException("AddInstanceOf() can only be used with composite types.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/>, the first <see cref="IObject"/> and the second <see cref="IObject"/> are compatible with
        /// <see cref="ICompositePredicate#AddBetween"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="firstObject">The first object.</param>
        /// <param name="secondObject">The second object.</param>
        public static void ValidateRoleBetween(IRoleType role, object firstObject, object secondObject)
        {
            if (role.ObjectType is IComposite)
            {
                throw new ArgumentException("AddBetween() can only be used with unit types.");
            }

            if (firstObject == null || secondObject == null)
            {
                throw new ArgumentException(
                    "AddBetween() requires a first and second object, use AddLessThan() or AddGreaterThan() instead.");
            }

            var firstRole = firstObject as IRoleType;
            var secondRole = secondObject as IRoleType;
            if ((firstRole != null && !(firstRole.ObjectType is IUnit)) ||
                (secondRole != null && !(secondRole.ObjectType is IUnit)))
            {
                throw new ArgumentException("AddBetween() can only be used with roles having unit types.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the <see cref="Extent"/> are compatible with
        /// <see cref="ICompositePredicate#AddContainedIn"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="extent">The extent.</param>
        public static void ValidateRoleContainedIn(IRoleType role, Extent extent)
        {
            if (role.ObjectType is IUnit)
            {
                throw new ArgumentException("AddContainedIn() can only be used with composite types.");
            }

            if (extent == null)
            {
                throw new ArgumentException("AddContainedIn() requires a non-null extent.");
            }
        }

        public static void ValidateRoleContainedIn(IRoleType role, IEnumerable<IObject> enumerable)
        {
            if (role.ObjectType is IUnit)
            {
                throw new ArgumentException("AddContainedIn() can only be used with composite types.");
            }

            if (enumerable == null)
            {
                throw new ArgumentException("AddContainedIn() requires a non-null extent.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the <see cref="IObject"/> are compatible with
        /// <see cref="ICompositePredicate#AddContainedIn"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="allorsObject">The allors object.</param>
        public static void ValidateRoleContains(IRoleType role, IObject allorsObject)
        {
            if (role.IsOne)
            {
                throw new ArgumentException("AddContains() can only be used when the role has multiplicity many.");
            }

            if (allorsObject == null)
            {
                throw new ArgumentException("AddContains() requires a non-null contained object.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the object are compatible with
        /// <see cref="ICompositePredicate#AddEquals"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="compareObject">The compare object.</param>
        public static void ValidateRoleEquals(IRoleType role, object compareObject)
        {
            if (role.IsMany)
            {
                throw new ArgumentException("AddRoleEqual() can only be used when the role has multiplicity one.");
            }

            if (compareObject == null)
            {
                throw new ArgumentException(
                    "AddEquals() requires a non-null value or object, use AddNot().AddExists() instead.");
            }

            var compareRole = compareObject as IRoleType;
            if (compareRole != null && compareRole.ObjectType is IComposite)
            {
                throw new ArgumentException("AddRoleEqual() for composites can only be used with objects (not other roles).");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> is compatible with
        /// <see cref="ICompositePredicate#Exists"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        public static void ValidateRoleExists(IRoleType role)
        {
            // TODO: ?
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the unit are compatible with
        /// <see cref="ICompositePredicate#AddGreaterThan"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="unit">The unit .</param>
        public static void ValidateRoleGreaterThan(IRoleType role, object unit)
        {
            if (role.ObjectType is IComposite)
            {
                throw new ArgumentException("AddGreaterThan() can only be used with unit types.");
            }

            if (unit == null)
            {
                throw new ArgumentException("AddGreaterThan() requires a non-null value.");
            }

            var compareRole = unit as IRoleType;
            if (compareRole != null && compareRole.ObjectType is IComposite)
            {
                throw new ArgumentException("AAddGreaterThan() can only be used with roles having unit types.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the <see cref="IObjectType"/> are compatible with
        /// <see cref="ICompositePredicate#AddInstanceOf"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="objectType">Type object type.</param>
        public static void ValidateRoleInstanceOf(IRoleType role, IObjectType objectType)
        {
            if (objectType is IUnit)
            {
                throw new ArgumentException("AddInstanceOf() can only be used with composite types.");
            }

            if (role.IsMany)
            {
                throw new ArgumentException("AddInstanceOf() can only be used with roles having multiplicity one.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the unit are compatible with
        /// <see cref="ICompositePredicate#AddLessThan"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="unit">The unit .</param>
        public static void ValidateRoleLessThan(IRoleType role, object unit)
        {
            if (role.ObjectType is IComposite)
            {
                throw new ArgumentException("AddLessThan() can only be used with unit types.");
            }

            if (unit == null)
            {
                throw new ArgumentException("AddLessThan() requires a value.");
            }

            var compareRole = unit as IRoleType;
            if (compareRole != null && compareRole.ObjectType is IComposite)
            {
                throw new ArgumentException("AddLessThan() can only be used with roles having unit types.");
            }
        }

        /// <summary>
        /// Asserts that the <see cref="RoleType"/> and the unit are compatible with
        /// <see cref="ICompositePredicate#AddLike"/>.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <param name="unit">The unit .</param>
        public static void ValidateRoleLikeFilter(IRoleType role, string unit)
        {
            var unitType = role.ObjectType as IUnit;
            if (unitType == null || !unitType.IsString)
            {
                throw new ArgumentException("AddLike() can only be used with String.");
            }

            if (unit == null)
            {
                throw new ArgumentException("AddLike() requires a non-null value.");
            }
        }

        public static void ValidateEquals(IObject equals)
        {
        }
    }
}