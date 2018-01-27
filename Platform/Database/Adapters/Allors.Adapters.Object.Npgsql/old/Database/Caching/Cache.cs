// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cache.cs" company="Allors bvba">
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

namespace Allors.Adapters.Database.Caching
{
    using System.Collections.Generic;

    using Allors.Meta;

    /// <summary>
    /// The Cache holds a CachedObject and/or IObjectType by ObjectId.
    /// </summary>
    public sealed class Cache : ICache
    {
        private readonly HashSet<IObjectType> transientConcreteClasses; 

        private readonly Dictionary<long, CachedObject> cachedObjectByObjectId;
        private readonly Dictionary<long, IObjectType> objectTypeByObjectId;

        public Cache(IComposite[] transientObjectTypes)
        {
            this.cachedObjectByObjectId = new Dictionary<long, CachedObject>();
            this.objectTypeByObjectId = new Dictionary<long, IObjectType>();

            if (transientObjectTypes != null)
            {
                this.transientConcreteClasses = new HashSet<IObjectType>();
                foreach (var transientObjectType in transientObjectTypes)
                {
                    foreach (var transientConcreteClass in transientObjectType.LeafClasses)
                    {
                        this.transientConcreteClasses.Add(transientConcreteClass);
                    }
                }

                if (this.transientConcreteClasses.Count == 0)
                {
                    this.transientConcreteClasses = null;
                }
            }
        }

        /// <summary>
        /// Invalidates the Cache.
        /// </summary>
        public void Invalidate()
        {
            this.cachedObjectByObjectId.Clear();
            this.objectTypeByObjectId.Clear();
        }

        public ICachedObject GetOrCreateCachedObject(IObjectType concreteClass, long objectId, int localCacheId)
        {
            if (this.transientConcreteClasses != null && this.transientConcreteClasses.Contains(concreteClass))
            {
                return new CachedObject(localCacheId); 
            }

            CachedObject cachedObject;
            if (this.cachedObjectByObjectId.TryGetValue(objectId, out cachedObject))
            {
                if (!cachedObject.LocalCacheVersion.Equals(localCacheId))
                {
                    cachedObject = new CachedObject(localCacheId);
                    this.cachedObjectByObjectId[objectId] = cachedObject;
                }
            }
            else
            {
                cachedObject = new CachedObject(localCacheId);
                this.cachedObjectByObjectId[objectId] = cachedObject;
            }

            return cachedObject;
        }

        public IObjectType GetObjectType(long objectId)
        {
            IObjectType objectType;
            this.objectTypeByObjectId.TryGetValue(objectId, out objectType);
            return objectType;
        }

        public void SetObjectType(long objectId, IObjectType objectType)
        {
            this.objectTypeByObjectId[objectId] = objectType;
        }

        public void OnCommit(IList<long> accessedObjectIds, IList<long> changedObjectIds)
        {
            if (changedObjectIds.Count > 0)
            {
                foreach (var changedObjectId in changedObjectIds)
                {
                    this.cachedObjectByObjectId.Remove(changedObjectId);
                }
            }
        }

        public void OnRollback(IList<long> accessedObjectIds)
        {
        }
    }
}