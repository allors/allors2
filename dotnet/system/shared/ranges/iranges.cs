// <copyright file="IOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges
{
    using System;
    using System.Collections.Generic;

    public interface IRanges<T> where T : IComparable<T>
    {
        IRange<T> Import(IEnumerable<T>? unsortedItems);

        IRange<T> Load(IEnumerable<T>? sortedItems);

        IRange<T> Load(params T[] sortedItems);

        IRange<T> Load(T item);

        IRange<T> Ensure(object? nullable);

        IRange<T> Add(IRange<T>? range, T item);

        IRange<T> Remove(IRange<T>? range, T item);

        IRange<T> Union(IRange<T>? range, IRange<T>? other);

        IRange<T> Except(IRange<T>? range, IRange<T>? other);
    }
}
