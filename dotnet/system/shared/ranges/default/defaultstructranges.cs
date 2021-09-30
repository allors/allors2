// <copyright file="DefaultOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultStructRanges<T> : DefaultRanges<T> where T : struct, IComparable<T>
    {
        public override IRange<T> Union(IRange<T>? range, IRange<T>? other)
        {
            switch (range)
            {
                case null:
                case EmptyRange<T> _:
                    return other ?? EmptyRange<T>.Instance;
                case ArrayRange<T> _ when other == null:
                    return range;
                case ArrayRange<T> _ when other is EmptyRange<T>:
                    return range;
                case ArrayRange<T> arrayRange when other is ArrayRange<T> otherArrayRange:
                    switch (arrayRange.Items)
                    {
                        case var items when items.Length == 1:
                            return otherArrayRange.Items switch
                            {
                                var otherItems when otherItems.Length == 1 && items[0].CompareTo(otherItems[0]) == 0 => range,
                                var otherItems when otherItems.Length == 1 => items[0].CompareTo(otherItems[0]) < 0 ? new ArrayRange<T>(new[] { items[0], otherItems[0] }) : new ArrayRange<T>(new[] { otherItems[0], items[0] }),
                                _ => this.Add(other, items[0])
                            };

                        default:
                        {
                            var items = arrayRange.Items;

                            switch (otherArrayRange.Items)
                            {
                                case var otherItems when otherItems.Length == 1:
                                    return this.Add(range, otherItems[0]);
                                default:
                                    var otherArray = otherArrayRange.Items;
                                    var arrayLength = items.Length;
                                    var otherArrayLength = otherArray.Length;

                                    var result = new T[arrayLength + otherArrayLength];

                                    var i = 0;
                                    var j = 0;
                                    var k = 0;

                                    T previous = default;
                                    while (i < arrayLength && j < otherArrayLength)
                                    {
                                        var value = items[i];
                                        var otherValue = otherArray[j];

                                        if (value.CompareTo(otherValue) < 0)
                                        {
                                            if (value.CompareTo(previous) != 0)
                                            {
                                                result[k++] = value;
                                            }

                                            i++;
                                            previous = value;
                                        }
                                        else
                                        {
                                            if (Equals(previous, default) || otherValue.CompareTo(previous) != 0)
                                            {
                                                result[k++] = otherValue;
                                            }

                                            if (otherValue.CompareTo(value) == 0)
                                            {
                                                i++;
                                            }

                                            j++;
                                            previous = otherValue;
                                        }
                                    }

                                    if (i < arrayLength)
                                    {
                                        var rest = arrayLength - i;
                                        Array.Copy(items, i, result, k, rest);
                                        k += rest;
                                    }
                                    else if (j < otherArrayLength)
                                    {
                                        var rest = otherArrayLength - j;
                                        Array.Copy(otherArray, j, result, k, rest);
                                        k += rest;
                                    }

                                    if (k < result.Length)
                                    {
                                        Array.Resize(ref result, k);
                                    }

                                    return new ArrayRange<T>(result);
                            }
                        }
                    }

                default:
                    throw new ArgumentOutOfRangeException($"Range type {range.GetType()}");
            }
        }
    }
}
