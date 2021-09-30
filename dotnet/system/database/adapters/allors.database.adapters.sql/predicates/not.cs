// <copyright file="Not.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;

    using Meta;

    internal sealed class Not : Predicate, ICompositePredicate
    {
        private readonly ExtentFiltered extent;
        private Predicate filter;

        internal Not(ExtentFiltered extent)
        {
            this.extent = extent;
            if (extent.Strategy != null)
            {
                var allorsObject = extent.Strategy.GetObject();
                if (extent.AssociationType != null)
                {
                    var role = extent.AssociationType.RoleType;
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
                    var association = extent.RoleType.AssociationType;
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

        internal override bool Include => this.filter != null && this.filter.Include;

        public ICompositePredicate AddAnd()
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AndPredicate(this.extent);
            return (ICompositePredicate)this.filter;
        }

        public ICompositePredicate AddBetween(IRoleType role, object firstValue, object secondValue)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            if (firstValue is IRoleType betweenRoleA && secondValue is IRoleType betweenRoleB)
            {
                this.filter = new RoleBetweenRole(this.extent, role, betweenRoleA, betweenRoleB);
            }
            else if (firstValue is IAssociationType betweenAssociationA && secondValue is IAssociationType betweenAssociationB)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new RoleBetweenValue(this.extent, role, firstValue, secondValue);
            }

            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, Allors.Database.Extent containingExtent)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new RoleContainedInExtent(this.extent, role, containingExtent);
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, IEnumerable<IObject> containingEnumerable)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new NotRoleContainedInEnumerable(this.extent, role, containingEnumerable);
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, Allors.Database.Extent containingExtent)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new NotAssociationContainedInExtent(this.extent, association, containingExtent);
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, IEnumerable<IObject> containingEnumerable)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new NotAssociationContainedInEnumerable(this.extent, association, containingEnumerable);
            return this;
        }

        public ICompositePredicate AddContains(IRoleType role, IObject containedObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new RoleContains(this.extent, role, containedObject);
            return this;
        }

        public ICompositePredicate AddContains(IAssociationType association, IObject containedObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AssociationContains(this.extent, association, containedObject);
            return this;
        }

        public ICompositePredicate AddEquals(IObject allorsObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new Equals(allorsObject);
            return this;
        }

        public ICompositePredicate AddEquals(IRoleType role, object obj)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            if (obj is IRoleType equalsRole)
            {
                this.filter = new RoleEqualsRole(this.extent, role, equalsRole);
            }
            else if (obj is IAssociationType equalsAssociation)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new RoleEqualsValue(this.extent, role, obj);
            }

            return this;
        }

        public ICompositePredicate AddEquals(IAssociationType association, IObject allorsObject)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AssociationEquals(this.extent, association, allorsObject);
            return this;
        }

        public ICompositePredicate AddExists(IRoleType role)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new RoleExists(this.extent, role);
            return this;
        }

        public ICompositePredicate AddExists(IAssociationType association)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AssociationExists(this.extent, association);
            return this;
        }

        public ICompositePredicate AddGreaterThan(IRoleType role, object value)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            if (value is IRoleType greaterThanRole)
            {
                this.filter = new RoleGreaterThanRole(this.extent, role, greaterThanRole);
            }
            else if (value is IAssociationType greaterThanAssociation)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new RoleGreaterThanValue(this.extent, role, value);
            }

            return this;
        }

        public ICompositePredicate AddInstanceof(IComposite type)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new InstanceOf(type, CompositePredicate.GetConcreteSubClasses(type));
            return this;
        }

        public ICompositePredicate AddInstanceof(IRoleType role, IComposite type)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new RoleInstanceof(this.extent, role, type, CompositePredicate.GetConcreteSubClasses(type));
            return this;
        }

        public ICompositePredicate AddInstanceof(IAssociationType association, IComposite type)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new AssociationInstanceOf(this.extent, association, type, CompositePredicate.GetConcreteSubClasses(type));
            return this;
        }

        public ICompositePredicate AddLessThan(IRoleType role, object value)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            if (value is IRoleType lessThanRole)
            {
                this.filter = new RoleLessThanRole(this.extent, role, lessThanRole);
            }
            else if (value is IAssociationType lessThanAssociation)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.filter = new RoleLessThanValue(this.extent, role, value);
            }

            return this;
        }

        public ICompositePredicate AddLike(IRoleType role, string value)
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new RoleLike(this.extent, role, value);
            return this;
        }

        public ICompositePredicate AddNot()
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new Not(this.extent);
            return (ICompositePredicate)this.filter;
        }

        public ICompositePredicate AddOr()
        {
            this.CheckUnarity();
            this.extent.FlushCache();
            this.filter = new Or(this.extent);
            return (ICompositePredicate)this.filter;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            if (this.Include)
            {
                if (this.filter.IsNotFilter)
                {
                    this.filter.BuildWhere(statement, alias);
                }
                else
                {
                    statement.Append(" NOT (");
                    this.filter.BuildWhere(statement, alias);
                    statement.Append(")");
                }
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            if (this.filter != null)
            {
                this.filter.Setup(statement);
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
