// <copyright file="ISessionObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public interface INewSessionObject : ISessionObject
    {
        long? NewId { get; set; }
    }
}
