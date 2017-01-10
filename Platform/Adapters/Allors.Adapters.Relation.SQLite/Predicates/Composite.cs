//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateCompositeSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsPredicateCompositeSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections.Generic;

    using Allors.Meta;

    internal abstract class AllorsPredicateCompositeSql : AllorsPredicateSql, ICompositePredicate
    {
        protected readonly List<AllorsPredicateSql> filters;
        private readonly AllorsExtentFilteredSql extent;

        internal AllorsPredicateCompositeSql(AllorsExtentFilteredSql extent)
        {
            this.extent = extent;
            this.filters = new List<AllorsPredicateSql>(4);

            if (extent.strategy != null)
            {
                var allorsObject = extent.strategy.GetObject();
                if (extent.association != null)
                {
                    var role = extent.association.RoleType;
                    if (role.IsMany)
                    {
                        this.AddContains(role, allorsObject);
                    }
                    else
                    {
                        this.AddEquals(role, allorsObject);
                    }
                }
                else
                {
                    var association = extent.role.AssociationType;
                    if (association.IsMany)
                    {
                        this.AddContains(association, allorsObject);
                    }
                    else
                    {
                        this.AddEquals(association, allorsObject);
                    }
                }
            }
        }

        internal override bool Include
        {
            get
            {
                foreach (var filter in this.filters)
                {
                    if (filter.Include)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public ICompositePredicate AddAnd()
        {
            this.extent.FlushCache();
            var andFilter = new AllorsPredicateAndSql(this.extent);
            this.filters.Add(andFilter);
            return andFilter;
        }

        public ICompositePredicate AddBetween(IRoleType role, Object firstValue, Object secondValue)
        {
            this.extent.FlushCache();
            var betweenRoleA = firstValue as IRoleType;
            var betweenRoleB = secondValue as IRoleType;
            var betweenAssociationA = firstValue as IAssociationType;
            var betweenAssociationB = secondValue as IAssociationType;
            if (betweenRoleA != null && betweenRoleB != null)
            {
                this.filters.Add(new AllorsPredicateRoleBetweenRoleSql(this.extent, role, betweenRoleA, betweenRoleB));
            }
            else if (betweenAssociationA != null && betweenAssociationB != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filters.Add(new AllorsPredicateRoleBetweenValueSql(this.extent, role, firstValue, secondValue));
            }

            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, Extent containingExtent)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateRoleInExtentSql(this.extent, role, containingExtent));
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, IEnumerable<IObject> containingEnumerable)
        {
            this.extent.FlushCache();
            this.filters.Add(new RoleContainedInEnumerable(this.extent, role, containingEnumerable));
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, Extent containingExtent)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateAssociationInExtentSql(this.extent, association, containingExtent));
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, IEnumerable<IObject> containingEnumerable)
        {
            this.extent.FlushCache();
            this.filters.Add(new AssociationContainedInEnumerable(this.extent, association, containingEnumerable));
            return this;
        }

        public ICompositePredicate AddContains(IRoleType role, IObject containedObject)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateRoleContainsSql(this.extent, role, containedObject));
            return this;
        }

        public ICompositePredicate AddContains(IAssociationType association, IObject containedObject)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateAssociationContainsSql(this.extent, association, containedObject));
            return this;
        }

        public ICompositePredicate AddEquals(IObject allorsObject)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateEqualsSql(this.extent, allorsObject));
            return this;
        }

        public ICompositePredicate AddEquals(IRoleType role, Object obj)
        {
            this.extent.FlushCache();
            var equalsRole = obj as IRoleType;
            var equalsAssociation = obj as IAssociationType;
            if (equalsRole != null)
            {
                this.filters.Add(new AllorsPredicateRoleUnitEqualsRoleSql(this.extent, role, equalsRole));
            }
            else if (equalsAssociation != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (role.ObjectType is IUnit)
                {
                    this.filters.Add(new AllorsPredicateRoleUnitEqualsValueSql(this.extent, role, obj));
                }
                else
                {
                    this.filters.Add(new AllorsPredicateRoleCompositeEqualsSql(this.extent, role, obj));
                }
            }

            return this;
        }

        public ICompositePredicate AddEquals(IAssociationType association, IObject allorsObject)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateAssociationEqualsSql(this.extent, association, allorsObject));
            return this;
        }

        public ICompositePredicate AddExists(IRoleType role)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateRoleExistsSql(this.extent, role));
            return this;
        }

        public ICompositePredicate AddExists(IAssociationType association)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateAssociationExistsSql(this.extent, association));
            return this;
        }

        public ICompositePredicate AddGreaterThan(IRoleType role, Object value)
        {
            this.extent.FlushCache();
            var greaterThanRole = value as IRoleType;
            var greaterThanAssociation = value as IAssociationType;
            if (greaterThanRole != null)
            {
                this.filters.Add(new AllorsPredicateRoleGreaterThanSql(this.extent, role, greaterThanRole));
            }
            else if (greaterThanAssociation != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filters.Add(new AllorsPredicateRoleGreaterThanValueSql(this.extent, role, value));
            }

            return this;
        }

        public ICompositePredicate AddInstanceof(IComposite type)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateInstanceOfSql(this.extent, type, this.GetConcreteSubClasses(type)));
            return this;
        }

        public ICompositePredicate AddInstanceof(IRoleType role, IComposite type)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateRoleInstanceofSql(this.extent, role, type, this.GetConcreteSubClasses(type)));
            return this;
        }

        public ICompositePredicate AddInstanceof(IAssociationType association, IComposite type)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateAssociationInstanceofSql(this.extent, association, type, this.GetConcreteSubClasses(type)));
            return this;
        }

        public ICompositePredicate AddLessThan(IRoleType role, Object value)
        {
            this.extent.FlushCache();
            var lessThanRole = value as IRoleType;
            var lessThanAssociation = value as IAssociationType;
            if (lessThanRole != null)
            {
                this.filters.Add(new AllorsPredicateRoleLessThanRoleSql(this.extent, role, lessThanRole));
            }
            else if (lessThanAssociation != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filters.Add(new AllorsPredicateRoleLessThanValueSql(this.extent, role, value));
            }

            return this;
        }

        public ICompositePredicate AddLike(IRoleType role, string value)
        {
            this.extent.FlushCache();
            this.filters.Add(new AllorsPredicateRoleLikeSql(this.extent, role, value));
            return this;
        }

        public ICompositePredicate AddNot()
        {
            this.extent.FlushCache();
            var noneFilter = new AllorsPredicateNotSql(this.extent);
            this.filters.Add(noneFilter);
            return noneFilter;
        }

        public ICompositePredicate AddOr()
        {
            this.extent.FlushCache();
            var orFilter = new AllorsPredicateOrSql(this.extent);
            this.filters.Add(orFilter);
            return orFilter;
        }

        protected IClass[] GetConcreteSubClasses(IComposite type)
        {
            return new List<IClass>(type.Classes).ToArray();
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            foreach (AllorsPredicateSql filter in this.filters)
            {
                filter.Setup(extent, statement);
            }
        }
    }
}