// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlExtent.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.Npgsql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Meta;

    internal abstract class SqlExtent : Extent
    {
        private IList<long> objectIds;

        internal override SqlExtent ContainedInExtent => this;

        public override int Count => this.ObjectIds.Count;

        public override IObject First => this.ObjectIds.Count > 0 ? this.Session.State.GetOrCreateReferenceForExistingObject(this.ObjectIds[0], this.Session).Strategy.GetObject() : null;

        internal ExtentOperation ParentOperationExtent { get; set; }

        internal abstract Session Session { get; }

        internal ExtentSort Sorter { get; private set; }

        private IList<long> ObjectIds => this.objectIds ?? (this.objectIds = this.GetObjectIds());

        internal new IObject this[int index] => this.GetItem(index);

        public override Allors.Extent AddSort(IRoleType roleType)
        {
            return this.AddSort(roleType, SortDirection.Ascending);
        }

        public override Allors.Extent AddSort(IRoleType roleType, SortDirection direction)
        {
            this.LazyLoadFilter();
            this.FlushCache();
            if (this.Sorter == null)
            {
                this.Sorter = new ExtentSort(this.Session, roleType, direction);
            }
            else
            {
                this.Sorter.AddSort(roleType, direction);
            }

            return this;
        }

        public override bool Contains(object value)
        {
            if (value != null)
            {
                return this.ObjectIds.Contains(((IObject)value).Id);
            }

            return false;
        }

        public override void CopyTo(Array array, int index)
        {
            this.ToArray().CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            var references = this.Session.GetOrCreateReferencesForExistingObjects(this.ObjectIds);
            return new ExtentEnumerator(references);
        }

        public override int IndexOf(object value)
        {
            if (value != null)
            {
                return this.ObjectIds.IndexOf(((IObject)value).Id);
            }

            return -1;
        }

        public override IObject[] ToArray()
        {
            var clrType = this.Session.Database.GetDomainType(this.ObjectType);
            return this.ToArray(clrType);
        }

        public override IObject[] ToArray(Type type)
        {
            var objects = this.Session.Instantiate(this.ObjectIds);
            var array = Array.CreateInstance(type, objects.Length);
            Array.Copy(objects, array, objects.Length);
            return (IObject[])array;
        }

        internal abstract string BuildSql(ExtentStatement statement);

        internal void FlushCache()
        {
            this.objectIds = null;
            this.ParentOperationExtent?.FlushCache();
        }

        internal IObject InternalGetItem(int index)
        {
            return this.GetItem(index);
        }

        protected override IObject GetItem(int index)
        {
            var objectId = this.ObjectIds[index];
            return this.Session.State.GetOrCreateReferenceForExistingObject(objectId, this.Session).Strategy.GetObject();
        }

        protected abstract IList<long> GetObjectIds();

        protected abstract void LazyLoadFilter();
    }
}
