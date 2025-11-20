// <copyright file="PreparedExtents.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PreparedExtents
    {
        private UniquelyIdentifiableSticky<PreparedExtent> cache;

        public UniquelyIdentifiableSticky<PreparedExtent> Cache => this.cache ??= new UniquelyIdentifiableSticky<PreparedExtent>(this.Session);
    }
}
