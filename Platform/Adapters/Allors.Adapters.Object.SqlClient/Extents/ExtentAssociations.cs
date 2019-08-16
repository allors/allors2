// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentAssociations.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    internal class ExtentAssociations : Extent
    {
        private readonly Strategy strategy;
        private readonly IAssociationType associationType;

        private ExtentFiltered upgrade;

        internal ExtentAssociations(Strategy strategy, IAssociationType associationType)
        {
            this.strategy = strategy;
            this.associationType = associationType;
        }

        public override int Count
        {
            get
            {
                if (this.upgrade != null)
                {
                    return this.upgrade.Count;
                }

                return this.strategy.ExtentGetCompositeAssociations(this.associationType).Length;
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

                var associations = this.strategy.ExtentGetCompositeAssociations(this.associationType);
                if (associations.Length == 0)
                {
                    return null;
                }

                return this.strategy.Session.State.GetOrCreateReferenceForExistingObject(associations[0], this.strategy.Session).Strategy.GetObject();
            }
        }

        public override IComposite ObjectType => this.strategy.Class;

        internal override SqlExtent ContainedInExtent
        {
            get
            {
                this.LazyUpgrade();

                return this.upgrade;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            this.ToArray().CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            if (this.upgrade != null)
            {
                return this.upgrade.GetEnumerator();
            }

            var associations = this.strategy.ExtentGetCompositeAssociations(this.associationType);
            var references = this.strategy.Session.GetOrCreateReferencesForExistingObjects(associations);
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

            var associations = (IList<long>)this.strategy.ExtentGetCompositeAssociations(this.associationType);
            return associations.IndexOf(((IObject)value).Id);
        }

        public override IObject[] ToArray()
        {
            if (this.upgrade != null)
            {
                return this.upgrade.ToArray();
            }

            var clrType = this.strategy.Session.Database.GetDomainType(this.ObjectType);
            return this.ToArray(clrType);
        }

        public override IObject[] ToArray(Type type)
        {
            if (this.upgrade != null)
            {
                return this.upgrade.ToArray(type);
            }

            var associations = this.strategy.ExtentGetCompositeAssociations(this.associationType);
            var references = this.strategy.Session.GetOrCreateReferencesForExistingObjects(associations);

            var objects = new IObject[references.Length];
            for (var i = 0; i < objects.Length; i++)
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

            var associations = (IList<long>)this.strategy.ExtentGetCompositeAssociations(this.associationType);
            return associations.Contains(((IObject)value).Id);
        }

        protected override IObject GetItem(int index)
        {
            if (this.upgrade != null)
            {
                return this.upgrade.InternalGetItem(index);
            }

            var associations = this.strategy.ExtentGetCompositeAssociations(this.associationType);
            return this.strategy.Session.State.GetOrCreateReferenceForExistingObject(associations[index], this.strategy.Session).Strategy.GetObject();
        }

        private void LazyUpgrade()
        {
            if (this.upgrade == null)
            {
                this.upgrade = new ExtentFiltered(this.strategy.Session, this.strategy, this.associationType);
            }
        }
    }
}