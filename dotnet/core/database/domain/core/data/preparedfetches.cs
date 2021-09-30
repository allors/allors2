// <copyright file="PreparedFetches.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PreparedFetches
    {
        private UniquelyIdentifiableSticky<PreparedFetch> cache;

        public UniquelyIdentifiableSticky<PreparedFetch> Cache => this.cache ??= new UniquelyIdentifiableSticky<PreparedFetch>(this.Session);
    }
}
