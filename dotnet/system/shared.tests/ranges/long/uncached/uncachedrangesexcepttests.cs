// <copyright file="UncachedRangesExceptTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    public class UncachedRangesExceptTests : RangesExceptTests
    {
        public override IRanges<long> Ranges { get; }

        public UncachedRangesExceptTests() => this.Ranges = new DefaultStructRanges<long>();
    }
}
