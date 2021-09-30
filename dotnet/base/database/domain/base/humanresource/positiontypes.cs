// <copyright file="PositionTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PositionTypes
    {
        private UniquelyIdentifiableSticky<PositionType> cache;

        private UniquelyIdentifiableSticky<PositionType> Cache => this.cache ??= new UniquelyIdentifiableSticky<PositionType>(this.Session);
    }
}
