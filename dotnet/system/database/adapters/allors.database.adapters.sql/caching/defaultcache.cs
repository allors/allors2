// <copyright file="DefaultCache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Meta;

    /// <summary>
    /// The Cache holds a CachedObject and/or IObjectType by ObjectId.
    /// </summary>
    public sealed class DefaultCache : ICache
    {
        private readonly HashSet<IClass> excludedClasses;

        private readonly ConcurrentDictionary<long, DefaultCachedObject> cachedObjectByObjectId;
        private readonly ConcurrentDictionary<long, IClass> objectTypeByObjectId;

        internal DefaultCache(IClass[] excludedClasses)
        {
            this.cachedObjectByObjectId = new ConcurrentDictionary<long, DefaultCachedObject>();
            this.objectTypeByObjectId = new ConcurrentDictionary<long, IClass>();

            if (excludedClasses != null)
            {
                this.excludedClasses = new HashSet<IClass>();
                foreach (var transientObjectType in excludedClasses)
                {
                    foreach (var transientClass in transientObjectType.DatabaseClasses)
                    {
                        this.excludedClasses.Add(transientClass);
                    }
                }

                if (this.excludedClasses.Count == 0)
                {
                    this.excludedClasses = null;
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

        public ICachedObject GetOrCreateCachedObject(IClass concreteClass, long objectId, long version)
        {
            if (this.excludedClasses != null && this.excludedClasses.Contains(concreteClass))
            {
                return new DefaultCachedObject(version);
            }

            if (this.cachedObjectByObjectId.TryGetValue(objectId, out var cachedObject))
            {
                if (!cachedObject.Version.Equals(version))
                {
                    cachedObject = new DefaultCachedObject(version);
                    this.cachedObjectByObjectId[objectId] = cachedObject;
                }
            }
            else
            {
                cachedObject = new DefaultCachedObject(version);
                this.cachedObjectByObjectId[objectId] = cachedObject;
            }

            return cachedObject;
        }

        public IClass GetObjectType(long objectId)
        {
            this.objectTypeByObjectId.TryGetValue(objectId, out var objectType);
            return objectType;
        }

        public void SetObjectType(long objectId, IClass objectType) => this.objectTypeByObjectId[objectId] = objectType;

        public void OnCommit(IList<long> accessedObjectIds, IList<long> changedObjectIds)
        {
            if (changedObjectIds.Count > 0)
            {
                foreach (var changedObjectId in changedObjectIds)
                {
                    this.cachedObjectByObjectId.TryRemove(changedObjectId, out var removedObject);
                }
            }
        }

        public void OnRollback(List<long> accessedObjectIds)
        {
        }
    }
}
