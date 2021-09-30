// <copyright file="UncachedRangesFromTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    using System.Runtime.CompilerServices;

    public class UncachedRangesLoadTests : RangesLoadTests
    {
        public override IRanges<long> Ranges { get; }

        public UncachedRangesLoadTests() => this.Ranges = new DefaultStructRanges<long>();
    }
}
