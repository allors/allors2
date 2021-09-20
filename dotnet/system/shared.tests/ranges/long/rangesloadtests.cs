// <copyright file="RangesFromTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    using System;
    using System.Linq;
    using Xunit;

    public abstract class RangesLoadTests
    {
        public abstract IRanges<long> Ranges { get; }

        [Fact]
        public void LoadDefault()
        {
            var num = this.Ranges;

            var x = num.Load();

            Assert.Equal(Array.Empty<long>(), x);
        }

        [Fact]
        public void LoadValue()
        {
            var num = this.Ranges;

            var x = num.Load(1L);

            Assert.Equal(new[] { 1L }, x);
        }

        [Fact]
        public void LoadPair()
        {
            var num = this.Ranges;

            var x = num.Load(1L, 2L);

            Assert.Equal(new[] { 1L, 2L }, x);
        }

        [Fact]
        public void LoadDistinctIterator()
        {
            var num = this.Ranges;

            var distinctIterator = Array.Empty<long>().Distinct();

            var x = num.Load(distinctIterator);

            Assert.True(x.IsEmpty);
            Assert.Null(x.Save());
        }
    }
}
