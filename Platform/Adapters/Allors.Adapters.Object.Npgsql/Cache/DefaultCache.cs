// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultCache.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.Npgsql.Caching
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Allors.Meta;

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
                    foreach (var transientConcreteClass in transientObjectType.Classes)
                    {
                        this.excludedClasses.Add(transientConcreteClass);
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
