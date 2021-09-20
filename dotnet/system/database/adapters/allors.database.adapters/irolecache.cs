// <copyright file="IRoleCache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using Meta;

    public interface IRoleCache
    {
        bool TryGetUnit(long associationId, object cacheId, IRoleType roleType, out object role);

        void SetUnit(long associationId, object cacheId, IRoleType roleType, object role);

        bool TryGetComposite(long associationId, object cacheId, IRoleType roleType, out long? roleId);

        void SetComposite(long associationId, object cacheId, IRoleType roleType, long? roleId);

        bool TryGetComposites(long associationId, object cacheId, IRoleType roleType, out long[] roleIds);

        void SetComposites(long associationId, object cacheId, IRoleType roleType, long[] roleIds);

        void Invalidate();

        void Invalidate(long[] objectsToInvalidate);
    }
}
