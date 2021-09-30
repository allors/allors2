// <copyright file="IOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges
{
    using System;
    using System.Collections.Generic;

    public interface IRange<T> : IEquatable<IRange<T>>, IEnumerable<T> where T : IComparable<T>
    {
        bool IsEmpty { get; }

        bool Contains(T item);

        T[]? Save();
    }
}
