// <copyright file="UncachedRangesFromTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    public class UncachedRangesImportTests : RangesImportTests
    {
        public override IRanges<long> Ranges { get; }

        public UncachedRangesImportTests() => this.Ranges = new DefaultStructRanges<long>();
    }
}
