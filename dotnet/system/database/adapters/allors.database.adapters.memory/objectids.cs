// <copyright file="ObjectIds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    internal abstract class ObjectIds
    {
        internal abstract void AdjustCurrentId(long id);

        internal abstract long Next();

        internal abstract long Parse(string idString);

        internal abstract void Reset();
    }
}
