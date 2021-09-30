// <copyright file="Not.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;

    using Meta;

    internal sealed class Not : Predicate, ICompositePredicate
    {
        private readonly ExtentFiltered extent;
        private Predicate predicate;

        internal Not(ExtentFiltered extent) => this.extent = extent;

        internal override bool Include => this.predicate?.Include == true;

        public ICompositePredicate AddAnd()
        {
            this.CheckUnarity();
            this.predicate = new And(this.extent);
            this.extent.Invalidate();
            return (ICompositePredicate)this.predicate;
        }

        public ICompositePredicate AddBetween(IRoleType role, object firstValue, object secondValue)
        {
            this.CheckUnarity();
            this.predicate = new RoleBetween(this.extent, role, firstValue, secondValue);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, Allors.Database.Extent containingExtent)
        {
            this.CheckUnarity();
            if (role.IsMany)
            {
                this.predicate = new RoleManyContainedInExtent(this.extent, role, containingExtent);
            }
            else
            {
                this.predicate = new RoleOneContainedInExtent(this.extent, role, containingExtent);
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IRoleType role, IEnumerable<IObject> containingEnumerable)
        {
            this.CheckUnarity();
            if (role.IsMany)
            {
                this.predicate = new RoleManyContainedInEnumerable(this.extent, role, containingEnumerable);
            }
            else
            {
                this.predicate = new RoleOneContainedInEnumerable(this.extent, role, containingEnumerable);
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, Allors.Database.Extent containingExtent)
        {
            this.CheckUnarity();
            this.predicate = new AssociationContainedInExtent(this.extent, association, containingExtent);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContainedIn(IAssociationType association, IEnumerable<IObject> containingEnumerable)
        {
            this.CheckUnarity();
            this.predicate = new AssociationContainedInEnumerable(this.extent, association, containingEnumerable);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContains(IRoleType role, IObject containedObject)
        {
            this.CheckUnarity();
            this.predicate = new RoleContains(this.extent, role, containedObject);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddContains(IAssociationType association, IObject containedObject)
        {
            this.CheckUnarity();
            this.predicate = new AssociationContains(this.extent, association, containedObject);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IObject allorsObject)
        {
            this.CheckUnarity();
            this.predicate = new Equals(allorsObject);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IRoleType role, object obj)
        {
            this.CheckUnarity();
            if (role.ObjectType is IUnit)
            {
                this.predicate = new RoleUnitEquals(this.extent, role, obj);
            }
            else
            {
                this.predicate = new RoleCompositeEqualsValue(this.extent, role, obj);
            }

            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddEquals(IAssociationType association, IObject allorsObject)
        {
            this.CheckUnarity();
            this.predicate = new AssociationEquals(this.extent, association, allorsObject);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddExists(IRoleType role)
        {
            this.CheckUnarity();
            this.predicate = new RoleExists(this.extent, role);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddExists(IAssociationType association)
        {
            this.CheckUnarity();
            this.predicate = new AssociationExists(this.extent, association);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddGreaterThan(IRoleType role, object value)
        {
            this.CheckUnarity();
            this.predicate = new RoleGreaterThan(this.extent, role, value);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IComposite type)
        {
            this.CheckUnarity();
            this.predicate = new Instanceof(type);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IRoleType role, IComposite type)
        {
            this.CheckUnarity();
            this.predicate = new RoleInstanceof(this.extent, role, type);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddInstanceof(IAssociationType association, IComposite type)
        {
            this.CheckUnarity();
            this.predicate = new AssociationInstanceOf(this.extent, association, type);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddLessThan(IRoleType role, object value)
        {
            this.CheckUnarity();
            this.predicate = new RoleLessThan(this.extent, role, value);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddLike(IRoleType role, string value)
        {
            this.CheckUnarity();
            this.predicate = new RoleLike(this.extent, role, value);
            this.extent.Invalidate();
            return this;
        }

        public ICompositePredicate AddNot()
        {
            this.CheckUnarity();
            this.predicate = new Not(this.extent);
            this.extent.Invalidate();
            return (ICompositePredicate)this.predicate;
        }

        public ICompositePredicate AddOr()
        {
            this.CheckUnarity();
            this.predicate = new Or(this.extent);
            this.extent.Invalidate();
            return (ICompositePredicate)this.predicate;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            switch (this.predicate.Evaluate(strategy))
            {
                case ThreeValuedLogic.True:
                    return ThreeValuedLogic.False;

                case ThreeValuedLogic.False:
                    return ThreeValuedLogic.True;

                default:
                    return ThreeValuedLogic.Unknown;
            }
        }

        private void CheckUnarity()
        {
            if (this.predicate != null)
            {
                throw new ArgumentException("Not predicate accepts only 1 operator (unary)");
            }
        }
    }
}
