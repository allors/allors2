// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositePredicate.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Memory
{
    using System.Collections.Generic;
    using Allors.Meta;

    internal abstract class CompositePredicate : Predicate, ICompositePredicate
    {
        private readonly List<Predicate> predicates;
        private readonly ExtentFiltered extent;

        internal CompositePredicate(ExtentFiltered extent)
        {
            this.extent = extent;
            this.predicates = new List<Predicate>(4);
        }

        internal override bool Include
        {
            get
            {
                foreach (Predicate filter in this.predicates)
                {
                    if (filter.Include)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        protected internal List<Predicate> Filters => this.predicates;

        public ICompositePredicate AddAnd()
        {
            And andFilter = new And(this.extent);
            this.predicates.Add(andFilter);
            this.extent.Invalidate();
            return andFilter;
        }

        public ICompositePredicate AddBetween(IRoleType role, object firstValue, object secondValue)
        {
            this.predicates.Add(new RoleBetween(this.extent, role, firstValue, secondValue));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, Allors.Extent containingExtent)
        {
            if (role.IsMany)
            {
                this.predicates.Add(new RoleManyContainedInExtent(this.extent, role, containingExtent));
            }
            else
            {
                this.predicates.Add(new RoleOneContainedInExtent(this.extent, role, containingExtent));
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, IEnumerable<IObject> containingEnumerable)
        {
            if (role.IsMany)
            {
                this.predicates.Add(new RoleManyContainedInEnumerable(this.extent, role, containingEnumerable));
            }
            else
            {
                this.predicates.Add(new RoleOneContainedInEnumerable(this.extent, role, containingEnumerable));
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, Allors.Extent containingExtent)
        {
            this.predicates.Add(new AssociationContainedInExtent(this.extent, association, containingExtent));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, IEnumerable<IObject> containingEnumerable)
        {
            this.predicates.Add(new AssociationContainedInEnumerable(this.extent, association, containingEnumerable));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContains(IRoleType role, IObject containedObject)
        {
            this.predicates.Add(new RoleContains(this.extent, role, containedObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContains(IAssociationType association, IObject containedObject)
        {
            this.predicates.Add(new AssociationContains(this.extent, association, containedObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IObject allorsObject)
        {
            this.predicates.Add(new Equals(allorsObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IRoleType role, object obj)
        {
            if (role.ObjectType is IUnit)
            {
                this.predicates.Add(new RoleUnitEquals(this.extent, role, obj));
            }
            else
            {
                this.predicates.Add(new RoleCompositeEqualsValue(this.extent, role, obj));
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IAssociationType association, IObject allorsObject)
        {
            this.predicates.Add(new AssociationEquals(this.extent, association, allorsObject));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddExists(IRoleType role)
        {
            this.predicates.Add(new RoleExists(this.extent, role));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddExists(IAssociationType association)
        {
            this.predicates.Add(new AssociationExists(this.extent, association));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddGreaterThan(IRoleType role, object value)
        {
            this.predicates.Add(new RoleGreaterThan(this.extent, role, value));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IComposite type)
        {
            this.predicates.Add(new Instanceof(type));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IRoleType role, IComposite type)
        {
            this.predicates.Add(new RoleInstanceof(this.extent, role, type));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IAssociationType association, IComposite type)
        {
            this.predicates.Add(new AssociationInstanceOf(this.extent, association, type));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddLessThan(IRoleType role, object value)
        {
            this.predicates.Add(new RoleLessThan(this.extent, role, value));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddLike(IRoleType role, string value)
        {
            this.predicates.Add(new RoleLike(this.extent, role, value));
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddNot()
        {
            Not notFilter = new Not(this.extent);
            this.predicates.Add(notFilter);
            this.extent.Invalidate();
            return notFilter;
        }

        public ICompositePredicate AddOr()
        {
            Or orFilter = new Or(this.extent);
            this.predicates.Add(orFilter);
            this.extent.Invalidate();
            return orFilter;
        }
    }
}
