
// <copyright file="ExtentRoles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections;

    using Meta;

    internal class ExtentRoles : Extent
    {
        private readonly Strategy strategy;
        private readonly IRoleType roleType;

        private ExtentFiltered upgrade;

        internal ExtentRoles(Strategy strategy, IRoleType roleType)
        {
            this.strategy = strategy;
            this.roleType = roleType;
        }

        internal override SqlExtent ContainedInExtent
        {
            get
            {
                this.LazyUpgrade();

                return this.upgrade;
            }
        }

        public override int Count
        {
            get
            {
                if (this.upgrade != null)
                {
                    return this.upgrade.Count;
                }

                return this.strategy.ExtentRolesGetCount(this.roleType);
            }
        }

        public override ICompositePredicate Filter
        {
            get
            {
                this.LazyUpgrade();

                return this.upgrade.Filter;
            }
        }

        public override IObject First
        {
            get
            {
                if (this.upgrade != null)
                {
                    return this.upgrade.First;
                }

                return this.strategy.ExtentRolesFirst(this.roleType);
            }
        }

        public override IComposite ObjectType => this.strategy.Class;

        private Reference[] References
        {
            get
            {
                var roles = this.strategy.Roles.GetCompositesRole(this.roleType);
                return this.strategy.Session.GetOrCreateReferencesForExistingObjects(roles);
            }
        }

        public override void CopyTo(Array array, int index)
        {
            this.upgrade?.CopyTo(array, index);

            this.strategy.ExtentRolesCopyTo(this.roleType, array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            if (this.upgrade != null)
            {
                return this.upgrade.GetEnumerator();
            }

            var references = this.References;
            return new ExtentEnumerator(references);
        }

        public override int IndexOf(object value)
        {
            if (value == null)
            {
                return -1;
            }

            if (this.upgrade != null)
            {
                return this.upgrade.IndexOf(value);
            }

            return this.strategy.ExtentIndexOf(this.roleType, (IObject)value);
        }

        public override IObject[] ToArray()
        {
            if (this.upgrade != null)
            {
                return this.upgrade.ToArray();
            }

            var clrType = this.strategy.Session.Database.GetDomainType(this.roleType.ObjectType);
            return this.ToArray(clrType);
        }

        public override IObject[] ToArray(Type type)
        {
            if (this.upgrade != null)
            {
                return this.upgrade.ToArray(type);
            }

            var references = this.References;
            var objects = (IObject[])Array.CreateInstance(type, references.Length);
            for (var i = 0; i < references.Length; i++)
            {
                objects[i] = references[i].Strategy.GetObject();
            }

            return objects;
        }

        public override Allors.Extent AddSort(IRoleType sort)
        {
            this.LazyUpgrade();

            this.upgrade.AddSort(sort);

            return this;
        }

        public override Allors.Extent AddSort(IRoleType sort, SortDirection direction)
        {
            this.LazyUpgrade();

            this.upgrade.AddSort(sort, direction);

            return this;
        }

        public override bool Contains(object value)
        {
            if (this.upgrade != null)
            {
                return this.upgrade.Contains(value);
            }

            return this.strategy.ExtentRolesContains(this.roleType, (IObject)value);
        }

        protected override IObject GetItem(int index)
        {
            if (this.upgrade != null)
            {
                return this.upgrade.InternalGetItem(index);
            }

            return this.strategy.ExtentGetItem(this.roleType, index);
        }

        private void LazyUpgrade()
        {
            if (this.upgrade == null)
            {
                this.upgrade = new ExtentFiltered(this.strategy.Session, this.strategy, this.roleType);
            }
        }
    }
}
