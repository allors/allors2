// <copyright file="CompositePredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using Meta;

    internal abstract class CompositePredicate : Predicate, ICompositePredicate
    {
        private readonly ExtentFiltered extent;

        internal CompositePredicate(ExtentFiltered extent)
        {
            this.extent = extent;
            this.Filters = new List<Predicate>(4);
        }

        internal override bool Include
        {
            get
            {
                foreach (var filter in this.Filters)
                {
                    if (filter.Include)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        protected internal List<Predicate> Filters { get; }

        public ICompositePredicate AddAnd()
        {
            var andFilter = new And(this.extent);
            this.Filters.Add(andFilter);
            this.extent.Invalidate();
            return andFilter;
        }

        public ICompositePredicate AddBetween(IRoleType role, object firstValue, object secondValue)
        {
            this.Filters.Add(new RoleBetween(this.extent, role, firstValue, secondValue));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, Allors.Database.Extent containingExtent)
        {
            if (role.IsMany)
            {
                this.Filters.Add(new RoleManyContainedInExtent(this.extent, role, containingExtent));
            }
            else
            {
                this.Filters.Add(new RoleOneContainedInExtent(this.extent, role, containingExtent));
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, IEnumerable<IObject> containingEnumerable)
        {
            if (role.IsMany)
            {
                this.Filters.Add(new RoleManyContainedInEnumerable(this.extent, role, containingEnumerable));
            }
            else
            {
                this.Filters.Add(new RoleOneContainedInEnumerable(this.extent, role, containingEnumerable));
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, Allors.Database.Extent containingExtent)
        {
            this.Filters.Add(new AssociationContainedInExtent(this.extent, association, containingExtent));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, IEnumerable<IObject> containingEnumerable)
        {
            this.Filters.Add(new AssociationContainedInEnumerable(this.extent, association, containingEnumerable));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContains(IRoleType role, IObject containedObject)
        {
            this.Filters.Add(new RoleContains(this.extent, role, containedObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContains(IAssociationType association, IObject containedObject)
        {
            this.Filters.Add(new AssociationContains(this.extent, association, containedObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IObject allorsObject)
        {
            this.Filters.Add(new Equals(allorsObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IRoleType role, object obj)
        {
            if (role.ObjectType is IUnit)
            {
                this.Filters.Add(new RoleUnitEquals(this.extent, role, obj));
            }
            else
            {
                this.Filters.Add(new RoleCompositeEqualsValue(this.extent, role, obj));
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IAssociationType association, IObject allorsObject)
        {
            this.Filters.Add(new AssociationEquals(this.extent, association, allorsObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddExists(IRoleType role)
        {
            this.Filters.Add(new RoleExists(this.extent, role));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddExists(IAssociationType association)
        {
            this.Filters.Add(new AssociationExists(this.extent, association));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddGreaterThan(IRoleType role, object value)
        {
            this.Filters.Add(new RoleGreaterThan(this.extent, role, value));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IComposite type)
        {
            this.Filters.Add(new Instanceof(type));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IRoleType role, IComposite type)
        {
            this.Filters.Add(new RoleInstanceof(this.extent, role, type));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IAssociationType association, IComposite type)
        {
            this.Filters.Add(new AssociationInstanceOf(this.extent, association, type));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddLessThan(IRoleType role, object value)
        {
            this.Filters.Add(new RoleLessThan(this.extent, role, value));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddLike(IRoleType role, string value)
        {
            this.Filters.Add(new RoleLike(this.extent, role, value));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddNot()
        {
            var notFilter = new Not(this.extent);
            this.Filters.Add(notFilter);
            this.extent.Invalidate();
            return notFilter;
        }

        public ICompositePredicate AddOr()
        {
            var orFilter = new Or(this.extent);
            this.Filters.Add(orFilter);
            this.extent.Invalidate();
            return orFilter;
        }
    }
}
