
// <copyright file="IOperator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Ranges
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class EmptyRange<T> : IRange<T> where T : IComparable<T>
    {
        private static readonly EmptyEnumerator<T> Enumerator = new EmptyEnumerator<T>();

        public static readonly EmptyRange<T> Instance = new EmptyRange<T>();

        private EmptyRange() { }

        public bool Equals(IRange<T> other) => ReferenceEquals(this, other);

        public IEnumerator<T> GetEnumerator() => Enumerator;

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool IsEmpty => true;

        public bool Contains(T item) => false;

        public T[]? Save() => null;

        public override string ToString() => "[]";

        private class EmptyEnumerator<T> : IEnumerator<T> where T : IComparable<T>
        {
            public bool MoveNext() => false;

            public void Reset() { }

            T IEnumerator<T>.Current => throw new NotSupportedException("Range is empty.");
            public object Current => throw new NotSupportedException("Range is empty.");

            public void Dispose() { }
        }
    }
}
