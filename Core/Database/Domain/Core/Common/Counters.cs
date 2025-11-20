// <copyright file="Counters.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors;

    public partial class Counters
    {
        private UniquelyIdentifiableSticky<Counter> cache;

        private UniquelyIdentifiableSticky<Counter> Cache => this.cache ??= new UniquelyIdentifiableSticky<Counter>(this.Session);
    }
}
