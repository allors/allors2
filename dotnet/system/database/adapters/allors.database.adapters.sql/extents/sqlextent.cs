// <copyright file="SqlExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Meta;

    internal abstract class SqlExtent : Extent
    {
        private IList<long> objectIds;

        public override int Count => this.ObjectIds.Count;

        internal override SqlExtent ContainedInExtent => this;

        internal ExtentOperation ParentOperationExtent { get; set; }

        internal abstract Transaction Transaction { get; }

        internal ExtentSort Sorter { get; private set; }

        private IList<long> ObjectIds => this.objectIds ??= this.GetObjectIds();

        internal new IObject this[int index] => this.GetItem(index);

        public override Allors.Database.Extent AddSort(IRoleType roleType) => this.AddSort(roleType, SortDirection.Ascending);

        public override Allors.Database.Extent AddSort(IRoleType roleType, SortDirection direction)
        {
            this.LazyLoadFilter();
            this.FlushCache();
            if (this.Sorter == null)
            {
                this.Sorter = new ExtentSort(this.Transaction, roleType, direction);
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

        public override void CopyTo(Array array, int index) => this.ToArray().CopyTo(array, index);

        public override IEnumerator GetEnumerator()
        {
            var references = this.Transaction.GetOrCreateReferencesForExistingObjects(this.ObjectIds);
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
            var clrType = this.Transaction.Database.GetDomainType(this.ObjectType);
            return this.ToArray(clrType);
        }

        public override IObject[] ToArray(Type type)
        {
            var objects = this.Transaction.Instantiate(this.ObjectIds);
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

        internal IObject InternalGetItem(int index) => this.GetItem(index);

        protected override IObject GetItem(int index)
        {
            var objectId = this.ObjectIds[index];
            return this.Transaction.State.GetOrCreateReferenceForExistingObject(objectId, this.Transaction).Strategy.GetObject();
        }

        protected abstract IList<long> GetObjectIds();

        protected abstract void LazyLoadFilter();
    }
}
