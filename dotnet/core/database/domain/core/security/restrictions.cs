// <copyright file="Restrictions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Restrictions
    {
        private UniquelyIdentifiableSticky<Restriction> cache;

        public Sticky<Guid, Restriction> Cache => this.cache ??= new UniquelyIdentifiableSticky<Restriction>(this.Session);
    }
}
