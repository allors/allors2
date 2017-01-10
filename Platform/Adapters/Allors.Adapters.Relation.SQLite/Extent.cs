//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsExtentSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections;

    using Allors.Meta;

    public abstract class AllorsExtentSql : Extent
    {
        private ObjectId[] objectIds;

        private AllorsExtentSortSql sorter;

        public override int Count
        {
            get { return this.ObjectIds.Length; }
        }

        public override IObject First
        {
            get
            {
                if (this.ObjectIds.Length > 0)
                {
                    var firstObjectId = this.ObjectIds[0];
                    return this.GetObject(firstObjectId);
                }

                return null;
            }
        }

        internal AllorsExtentOperationSql ParentOperationExtent { get; set; }

        internal abstract Session Session { get; }

        internal AllorsExtentSortSql Sorter
        {
            get { return this.sorter; }
        }

        private ObjectId[] ObjectIds
        {
            get
            {
                return this.objectIds ?? (this.objectIds = this.GetObjectIds());
            }
        }

        public new IObject this[int index]
        {
            get { return this.GetItem(index); }
        }
        
        public override Extent AddSort(IRoleType roleType)
        {
            return this.AddSort(roleType, SortDirection.Ascending);
        }

        public override Extent AddSort(IRoleType roleType, SortDirection sortDirection)
        {
            this.LazyLoadFilter(); // This will upgrade a strategy extent to a full extent
            this.FlushCache();
            if (this.sorter == null)
            {
                this.sorter = new AllorsExtentSortSql(this.Session, roleType, sortDirection);
            }
            else
            {
                this.sorter.AddSort(roleType, sortDirection);
            }

            return this;
        }

        public override bool Contains(object value)
        {
            if (value != null)
            {
                var obj = (IObject)value;
                return Array.IndexOf(this.ObjectIds, obj.Id) >= 0;
            }

            return false;
        }

        public override void CopyTo(Array array, int index)
        {
            for (var i = index; i < array.Length; i++)
            {
                var objectId = this.ObjectIds[i];
                var obj = this.GetObject(objectId);
                array.SetValue(obj, i);
            }
        }

        public override IEnumerator GetEnumerator()
        {
            foreach (var objectId in this.ObjectIds)
            {
                yield return this.GetObject(objectId);
            }
        }

        public override int IndexOf(object value)
        {
            return Array.IndexOf(this.ObjectIds, ((IObject)value).Id);
        }

        public override IObject[] ToArray()
        {
            var clrType = this.Session.Database.ObjectFactory.GetTypeForObjectType(this.ObjectType);
            var array = (IObject[])Array.CreateInstance(clrType, this.ObjectIds.Length);
            for (var i = 0; i < array.Length; i++)
            {
                var objectId = this.ObjectIds[i];
                array[i] = this.GetObject(objectId);
            }

            return array;
        }

        public override IObject[] ToArray(Type type)
        {
            var array = (IObject[])Array.CreateInstance(type, this.ObjectIds.Length);
            for (var i = 0; i < array.Length; i++)
            {
                var objectId = this.ObjectIds[i];
                array[i] = this.GetObject(objectId);
            }

            return array;
        }

        internal abstract string BuildSql(AllorsExtentStatementSql statement);

        internal void FlushCache()
        {
            this.objectIds = null;
            if (this.ParentOperationExtent != null)
            {
                this.ParentOperationExtent.FlushCache();
            }
        }

        protected override IObject GetItem(int index)
        {
            var objectId = this.ObjectIds[index];
            return this.GetObject(objectId);
        }

        protected abstract ObjectId[] GetObjectIds();

        protected abstract void LazyLoadFilter();

        private IObject GetObject(ObjectId objectId)
        {
            return new Strategy(this.Session, objectId).GetObject();
        }
    }
}