// <copyright file="IPreparedExtents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Services
{
    using System;
    using Data;

    public interface IPreparedExtents
    {
        IExtent Get(Guid id);
    }
}
