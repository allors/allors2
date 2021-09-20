// <copyright file="UncachedRangesRemoveTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    public class UncachedRangesRemoveTests : RangesRemoveTests
    {
        public override IRanges<long> Ranges { get; }

        public UncachedRangesRemoveTests() => this.Ranges = new DefaultStructRanges<long>();
    }
}
