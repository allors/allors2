// <copyright file="IOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ArrayRange<T> : IRange<T> where T : IComparable<T>
    {
        public ArrayRange(T[] items) => this.Items = items;

        internal T[] Items { get; }

        public bool Equals(IRange<T> other)
        {
            if (!(other is ArrayRange<T> otherArrayRange))
            {
                return false;
            }

            var otherItems = otherArrayRange.Items;

            if (ReferenceEquals(this.Items, otherItems))
            {
                return true;
            }

            var itemsLength = this.Items.Length;

            if (itemsLength != otherItems.Length)
            {
                return false;
            }

            for (var i = 0; i < itemsLength; i++)
            {
                if (this.Items[i].CompareTo(otherItems[i]) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj) =>
            obj switch
            {
                null => false,
                IRange<T> range => this.Equals(range),
                _ => throw new NotSupportedException($"Can not compare a Range with an object of type {obj.GetType()}")
            };

        public override int GetHashCode() => this.Items.GetHashCode();

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this.Items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool IsEmpty => false;

        public bool Contains(T item) =>
            this.Items switch
            {
                var singleItems when singleItems.Length == 1 => singleItems[0].CompareTo(item) == 0,
                _ => Array.BinarySearch(this.Items, item) >= 0,
            };

        public T[]? Save() => this.Items;

        public override string ToString() => "[" + string.Join(", ", this.Items) + "]";
    }
}
