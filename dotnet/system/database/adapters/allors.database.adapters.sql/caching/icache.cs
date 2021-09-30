// <copyright file="ICache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching
{
    using System.Collections.Generic;

    using Meta;

    /// <summary>
    /// The Cache holds a CachedObject and/or IObjectType by ObjectId.
    /// </summary>
    public interface ICache
    {
        ICachedObject GetOrCreateCachedObject(IClass concreteClass, long objectId, long version);

        IClass GetObjectType(long objectId);

        void SetObjectType(long objectId, IClass objectType);

        void OnCommit(IList<long> accessedObjectIds, IList<long> changedObjectIds);

        void OnRollback(List<long> accessedObjectIds);

        /// <summary>
        /// Invalidates the Cache.
        /// </summary>
        void Invalidate();
    }
}
