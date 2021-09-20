// <copyright file="RangesFromTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges.Long
{
    using System;
    using System.Linq;
    using Xunit;

    public abstract class RangesImportTests
    {
        public abstract IRanges<long> Ranges { get; }

        [Fact]
        public void ImportEmpty()
        {
            var num = this.Ranges;

            var x = num.Import(Array.Empty<long>());

            Assert.Equal(Array.Empty<long>(), x);
        }

        [Fact]
        public void ImportSingle()
        {
            var num = this.Ranges;

            var x = num.Import(new[] { 1L });

            Assert.Equal(new[] { 1L }, x);
        }

        [Fact]
        public void ImportOrderedPair()
        {
            var num = this.Ranges;

            var x = num.Import(new[] { 1L, 2L });

            Assert.Equal(new[] { 1L, 2L }, x);
        }

        [Fact]
        public void ImportUnorderedPair()
        {
            var num = this.Ranges;

            var x = num.Import(new[] { 2L, 1L });

            Assert.Equal(new[] { 1L, 2L }, x);
        }

        [Fact]
        public void ImportDistinctIterator()
        {
            var num = this.Ranges;

            var distinctIterator = Array.Empty<long>().Distinct();

            var x = num.Import(distinctIterator);

            Assert.True(x.IsEmpty);
            Assert.Null(x.Save());
        }
    }
}
