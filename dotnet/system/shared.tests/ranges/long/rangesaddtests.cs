// <copyright file="RangesAddTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    using Xunit;

    public abstract class RangesAddTests
    {
        public abstract IRanges<long> Ranges { get; }

        [Fact]
        public void Null()
        {
            var num = this.Ranges;

            var x = num.Load();
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { 2 }, z);
        }

        [Fact]
        public void ValueBefore()
        {
            var num = this.Ranges;

            var x = num.Load(1);
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { 1, 2 }, z);
        }

        [Fact]
        public void ValueAfter()
        {
            var num = this.Ranges;

            var x = num.Load(3);
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { 2, 3 }, z);
        }

        [Fact]
        public void ValueSame()
        {
            var num = this.Ranges;

            var x = num.Load(2);
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { 2 }, z);
        }

        [Fact]
        public void PairBefore()
        {
            var num = this.Ranges;

            var x = num.Load(-2, 1);
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { -2, 1, 2 }, z);
        }

        [Fact]
        public void PairAfter()
        {
            var num = this.Ranges;

            var x = num.Load(3, 4);
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { 2, 3, 4 }, z);
        }

        [Fact]
        public void PairOverlapping()
        {
            var num = this.Ranges;

            var x = num.Load(1, 3);
            const int y = 2;
            var z = num.Add(x, y);

            Assert.Equal(new long[] { 1, 2, 3 }, z);
        }
    }
}
