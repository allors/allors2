// <copyright file="DefaultOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class DefaultRanges<T> : IRanges<T> where T : IComparable<T>
    {
        public IRange<T> Load(IEnumerable<T>? sortedItems)
        {
            switch (sortedItems)
            {
                case null:
                case T[] { Length: 0 }:
                    return EmptyRange<T>.Instance;
                case T[] array:
                    return new ArrayRange<T>(array);
                case ICollection<T> collection:
                    var newArray = new T[collection.Count];
                    collection.CopyTo(newArray, 0);
                    return new ArrayRange<T>(newArray);
                default:
                    var materialized = sortedItems.ToArray();
                    if (materialized.Length == 0)
                    {
                        return EmptyRange<T>.Instance;
                    }

                    return new ArrayRange<T>(materialized);
            }
        }

        public IRange<T> Load(T item) => new ArrayRange<T>(new[] { item });

        public IRange<T> Load(params T[] sortedItems) =>
            sortedItems switch
            {
                null => EmptyRange<T>.Instance,
                { Length: 0 } => EmptyRange<T>.Instance,
                _ => new ArrayRange<T>(sortedItems)
            };

        public IRange<T> Ensure(object? nullable) => nullable switch
        {
            null => EmptyRange<T>.Instance,
            IRange<T> range => range,
            _ => throw new NotSupportedException($"Ensure is not supported from {nullable.GetType()}")
        };

        public IRange<T> Import(IEnumerable<T>? unsortedItems)
        {
            switch (unsortedItems)
            {
                case null:
                case T[] { Length: 0 }:
                    return EmptyRange<T>.Instance;
                case T[] array:
                    var sortedArray = (T[])array.Clone();
                    Array.Sort(sortedArray);
                    return new ArrayRange<T>(sortedArray);
                case ICollection<T> collection:
                    var newSortedArray = new T[collection.Count];
                    collection.CopyTo(newSortedArray, 0);
                    Array.Sort(newSortedArray);
                    return new ArrayRange<T>(newSortedArray);
                default:
                    var materialized = unsortedItems.ToArray();
                    if (materialized.Length == 0)
                    {
                        return EmptyRange<T>.Instance;
                    }

                    Array.Sort(materialized);
                    return new ArrayRange<T>(materialized);
            }
        }

        public IRange<T> Add(IRange<T>? range, T item)
        {
            switch (range)
            {
                case null:
                case EmptyRange<T> _:
                    return this.Load(item);
                case ArrayRange<T> arrayRange:
                    switch (arrayRange.Items)
                    {
                        case var items when items.Length == 1 && items[0].CompareTo(item) == 0:
                            return range;
                        case var items when items.Length == 1:
                            return items[0].CompareTo(item) < 0 ? new ArrayRange<T>(new[] { items[0], item }) : new ArrayRange<T>(new[] { item, items[0] });
                        default:
                            var array = arrayRange.Items;
                            var index = Array.BinarySearch(array, item);

                            if (index >= 0)
                            {
                                return range;
                            }

                            index = ~index;

                            var result = new T[array.Length + 1];
                            result[index] = item;

                            if (index == 0)
                            {
                                Array.Copy(array, 0, result, 1, array.Length);
                            }
                            else if (index == array.Length)
                            {
                                Array.Copy(array, result, array.Length);
                            }
                            else
                            {
                                Array.Copy(array, result, index);
                                Array.Copy(array, index, result, index + 1, array.Length - index);
                            }

                            return new ArrayRange<T>(result);
                    }

                default:
                    throw new ArgumentOutOfRangeException($"Range type {range.GetType()}");
            }
        }

        public IRange<T> Remove(IRange<T>? range, T item)
        {
            switch (range)
            {
                case null:
                    return EmptyRange<T>.Instance;
                case EmptyRange<T> _:
                    return range;
                case ArrayRange<T> arrayRange:
                    switch (arrayRange.Items)
                    {
                        case null:
                        case var items when items.Length == 1 && items[0].CompareTo(item) == 0:
                            return EmptyRange<T>.Instance;
                        case var items when items.Length == 1:
                            return range;
                        default:
                            var array = arrayRange.Items;
                            var index = Array.BinarySearch(array, item);

                            if (index < 0)
                            {
                                return range;
                            }

                            var result = new T[array.Length - 1];

                            if (index == 0)
                            {
                                Array.Copy(array, 1, result, 0, array.Length - 1);
                            }
                            else if (index == array.Length)
                            {
                                Array.Copy(array, result, array.Length - 1);
                            }
                            else
                            {
                                Array.Copy(array, result, index);
                                Array.Copy(array, index + 1, result, index, array.Length - index - 1);
                            }

                            return new ArrayRange<T>(result);
                    }

                default:
                    throw new ArgumentOutOfRangeException($"Range type {range.GetType()}");
            }
        }

        public abstract IRange<T> Union(IRange<T>? range, IRange<T>? other);

        public IRange<T> Except(IRange<T>? range, IRange<T>? other)
        {
            if (other == null || other is EmptyRange<T>)
            {
                return range ?? EmptyRange<T>.Instance;
            }

            switch (range)
            {
                case null:
                    return EmptyRange<T>.Instance;
                case EmptyRange<T> _:
                    return range;
                case ArrayRange<T> arrayRange:
                {
                    var otherArrayRange = (ArrayRange<T>)other;

                    switch (arrayRange.Items)
                    {
                        case var items when items.Length == 1:
                            return otherArrayRange.Items switch
                            {
                                var otherItems when otherItems.Length == 1 && items[0].CompareTo(otherItems[0]) == 0 => EmptyRange<T>.Instance,
                                var otherItems when otherItems.Length == 1 => range,
                                _ => other.Contains(items[0]) ? EmptyRange<T>.Instance : range,
                            };

                        default:
                        {
                            var items = arrayRange.Items;

                            switch (otherArrayRange.Items)
                            {
                                case var otherItems when otherItems.Length == 1:
                                    return this.Remove(range, otherItems[0]);
                                default:
                                {
                                    var otherItems = otherArrayRange.Items;

                                    var itemsLength = items.Length;
                                    var otherArrayLength = otherItems.Length;

                                    var result = new T[itemsLength];
                                    var i = 0;
                                    var j = 0;
                                    var k = 0;

                                    while (i < itemsLength && j < otherArrayLength)
                                    {
                                        var value = items[i];
                                        var otherValue = otherItems[j];

                                        if (value.CompareTo(otherValue) < 0)
                                        {
                                            result[k++] = value;
                                            i++;
                                        }
                                        else if (value.CompareTo(otherValue) > 0)
                                        {
                                            j++;
                                        }
                                        else
                                        {
                                            i++;
                                        }
                                    }

                                    if (i < itemsLength)
                                    {
                                        var rest = itemsLength - i;
                                        Array.Copy(items, i, result, k, rest);
                                        k += rest;
                                    }

                                    if (k < result.Length)
                                    {
                                        Array.Resize(ref result, k);
                                    }

                                    return result.Length != 0 ? (IRange<T>)new ArrayRange<T>(result) : EmptyRange<T>.Instance;
                                }
                            }
                        }
                    }
                }

                default:
                    throw new ArgumentOutOfRangeException($"Range type {range.GetType()}");
            }
        }
    }
}
