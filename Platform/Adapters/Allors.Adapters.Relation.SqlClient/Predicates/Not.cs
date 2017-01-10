//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateNotSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateNotSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;

    internal sealed class AllorsPredicateNotSql : AllorsPredicateSql, ICompositePredicate
    {
        private readonly AllorsExtentFilteredSql extent;
        private AllorsPredicateSql filter;

        internal AllorsPredicateNotSql(AllorsExtentFilteredSql extent)
        {
            this.extent = extent;

            if (extent.strategy != null)
            {
                IObject allorsObject = extent.strategy.GetObject();
                if (extent.association != null)
                {
                    IRoleType role = extent.association.RoleType;
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
                    IAssociationType association = extent.role.AssociationType;
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
            get { return this.filter != null && this.filter.Include; }
        }

        public ICompositePredicate AddAnd()
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateAndSql(this.extent);
            return (ICompositePredicate)this.filter;
        }

        public ICompositePredicate AddBetween(IRoleType role, object firstValue, object secondValue)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            var betweenRoleA = firstValue as IRoleType;
            var betweenRoleB = secondValue as IRoleType;
            var betweenAssociationA = firstValue as IAssociationType;
            var betweenAssociationB = secondValue as IAssociationType;
            if (betweenRoleA != null && betweenRoleB != null)
            {
                this.filter = new AllorsPredicateRoleBetweenRoleSql(this.extent, role, betweenRoleA, betweenRoleB);
            }
            else if (betweenAssociationA != null && betweenAssociationB != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new AllorsPredicateRoleBetweenValueSql(this.extent, role, firstValue, secondValue);
            }

            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, Extent containingExtent)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateRoleInExtentSql(this.extent, role, containingExtent);
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, IEnumerable<IObject> containingEnumerable)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new RoleContainedInEnumerable(this.extent, role, containingEnumerable);
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, Extent containingExtent)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateAssociationInExtentSql(this.extent, association, containingExtent);
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, IEnumerable<IObject> containingEnumerable)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AssociationContainedInEnumerable(this.extent, association, containingEnumerable);
            return this;
        }

        public ICompositePredicate AddContains(IRoleType role, IObject containedObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateRoleContainsSql(this.extent, role, containedObject);
            return this;
        }

        public ICompositePredicate AddContains(IAssociationType association, IObject containedObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateAssociationContainsSql(this.extent, association, containedObject);
            return this;
        }

        public ICompositePredicate AddEquals(IObject allorsObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateEqualsSql(this.extent, allorsObject);
            return this;
        }

        public ICompositePredicate AddEquals(IRoleType role, object obj)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            var equalsRole = obj as IRoleType;
            var equalsAssociation = obj as IAssociationType;
            if (equalsRole != null)
            {
                this.filter = new AllorsPredicateRoleUnitEqualsRoleSql(this.extent, role, equalsRole);
            }
            else if (equalsAssociation != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (role.ObjectType is IUnit)
                {
                    this.filter = new AllorsPredicateRoleUnitEqualsValueSql(this.extent, role, obj);
                }
                else
                {
                    this.filter = new AllorsPredicateRoleCompositeEqualsSql(this.extent, role, obj);
                }
            }

            return this;
        }

        public ICompositePredicate AddEquals(IAssociationType association, IObject allorsObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateAssociationEqualsSql(this.extent, association, allorsObject);
            return this;
        }

        public ICompositePredicate AddExists(IRoleType role)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateRoleExistsSql(this.extent, role);
            return this;
        }

        public ICompositePredicate AddExists(IAssociationType association)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateAssociationExistsSql(this.extent, association);
            return this;
        }

        public ICompositePredicate AddGreaterThan(IRoleType role, object value)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            var greaterThanRole = value as IRoleType;
            var greaterThanAssociation = value as IAssociationType;
            if (greaterThanRole != null)
            {
                this.filter = new AllorsPredicateRoleGreaterThanSql(this.extent, role, greaterThanRole);
            }
            else if (greaterThanAssociation != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new AllorsPredicateRoleGreaterThanValueSql(extent, role, value);
            }

            return this;
        }

        public ICompositePredicate AddInstanceof(IComposite type)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateInstanceOfSql(this.extent, type, new List<IClass>(type.Classes).ToArray());
            return this;
        }

        public ICompositePredicate AddInstanceof(IRoleType role, IComposite type)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateRoleInstanceofSql(this.extent, role, type, new List<IClass>(type.Classes).ToArray());
            return this;
        }

        public ICompositePredicate AddInstanceof(IAssociationType association, IComposite type)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateAssociationInstanceofSql(this.extent, association, type, new List<IClass>(type.Classes).ToArray());
            return this;
        }

        public ICompositePredicate AddLessThan(IRoleType role, object value)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            var lessThanRole = value as IRoleType;
            var lessThanAssociation = value as IAssociationType;
            if (lessThanRole != null)
            {
                this.filter = new AllorsPredicateRoleLessThanRoleSql(this.extent, role, lessThanRole);
            }
            else if (lessThanAssociation != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new AllorsPredicateRoleLessThanValueSql(this.extent, role, value);
            }

            return this;
        }

        public ICompositePredicate AddLike(IRoleType role, string value)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateRoleLikeSql(this.extent, role, value);
            return this;
        }

        public ICompositePredicate AddNot()
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateNotSql(this.extent);
            return (ICompositePredicate)this.filter;
        }

        public ICompositePredicate AddOr()
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AllorsPredicateOrSql(this.extent);
            return (ICompositePredicate)this.filter;
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            if (this.Include)
            {
                statement.Append(" NOT (");
                this.filter.BuildWhere(extent, mapping, statement, type, alias);
                statement.Append(")");
            }

            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            if (this.filter != null)
            {
                this.filter.Setup(extent, statement);
            }
        }

        private void CheckUnarity()
        {
            if (this.filter != null)
            {
                throw new ArgumentException("Not predicate accepts only 1 operator (unary)");
            }
        }
    }
}