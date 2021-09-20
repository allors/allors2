// <copyright file="RangesUnionTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    using System;
    using Xunit;

    public abstract class RangesUnionTests
    {
        public abstract IRanges<long> Ranges { get; }

        [Fact]
        public void NullWithNull()
        {
            var num = this.Ranges;

            var x = num.Load();
            var y = num.Load();
            var z = num.Union(x, y);

            Assert.Equal(Array.Empty<long>(), z);
        }

        [Fact]
        public void ValueWithNull()
        {
            var num = this.Ranges;

            var x = num.Load(1);
            var y = num.Load();
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 1 }, z);
        }

        [Fact]
        public void PairWithNull()
        {
            var num = this.Ranges;

            var x = num.Load(1, 2);
            var y = num.Load();
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 1, 2 }, z);
        }

        [Fact]
        public void ValueDuplicates()
        {
            var num = this.Ranges;

            var x = num.Load(1);
            var y = num.Load(1);
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 1 }, z);
        }

        [Fact]
        public void PairDuplicates()
        {
            var num = this.Ranges;

            var x = num.Load(1, 2);
            var y = num.Load(1, 2);
            var z = num.Union(x, y);

            Assert.Equal(new ArrayRange<long>(new long[] { 1, 2 }), z);
        }


        [Fact]
        public void BeforePairWithPair()
        {
            var num = this.Ranges;

            var x = num.Load(1, 2);
            var y = num.Load(3, 4);
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 1, 2, 3, 4 }, z);
        }

        [Fact]
        public void AfterPairWithPair()
        {
            var num = this.Ranges;

            var x = num.Load(5, 6);
            var y = num.Load(3, 4);
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 3, 4, 5, 6 }, z);
        }


        [Fact]
        public void OverlappingPairWithPair()
        {
            var num = this.Ranges;

            var x = num.Load(2, 5);
            var y = num.Load(3, 4);
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 2, 3, 4, 5 }, z);
        }

        [Fact]
        public void BeforeIntertwinedPairWithPair()
        {
            var num = this.Ranges;

            var x = num.Load(2, 4);
            var y = num.Load(3, 5);
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 2, 3, 4, 5 }, z);
        }

        [Fact]
        public void AfterIntertwinedPairWithPair()
        {
            var num = this.Ranges;

            var x = num.Load(3, 5);
            var y = num.Load(2, 4);
            var z = num.Union(x, y);

            Assert.Equal(new long[] { 2, 3, 4, 5 }, z);
        }
    }
}
