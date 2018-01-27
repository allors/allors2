// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentRoles.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using System;
    using System.Collections;

    using Meta;

    public class ExtentRoles : Extent
    {
        private readonly Strategy strategy;
        private readonly IRoleType roleType;

        private ExtentFiltered upgrade;

        public ExtentRoles(Strategy strategy, IRoleType roleType)
        {
            this.strategy = strategy;
            this.roleType = roleType;
        }

        public override SqlExtent ContainedInExtent
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

        public override IComposite ObjectType
        {
            get
            {
                return this.strategy.Class;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            if (this.upgrade != null)
            {
                this.upgrade.CopyTo(array, index);
            }

            this.strategy.ExtentRolesCopyTo(this.roleType, array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            if (this.upgrade != null)
            {
                return this.upgrade.GetEnumerator();
            }

            var roles = this.strategy.Roles.GetCompositeRoles(this.roleType);
            var references = this.strategy.SqlSession.GetOrCreateAssociationsForExistingObjects(roles);
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
            
            var clrType = this.strategy.SqlSession.SqlDatabase.GetDomainType(this.roleType.ObjectType);
            return this.ToArray(clrType);
        }

        public override IObject[] ToArray(Type type)
        {
            if (this.upgrade != null)
            {
                return this.upgrade.ToArray(type);
            } 
            
            var objects = new ArrayList(this);
            return (IObject[])objects.ToArray(type);
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
                this.upgrade = new ExtentFiltered(this.strategy.SqlSession, this.strategy, this.roleType);
            }
        }
    }
}