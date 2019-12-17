// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    /// <summary>
    /// Provides storage for Database Save() and Load().
    /// </summary>
    public interface IStorage
    {
        IStorageContainer[] Containers { get; }

        IStorageContainer CreateContainer(Guid id);
    }
}
