// <copyright file="EmptySet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class EmptySet<T> : ISet<T>
    {
        public static readonly EmptySet<T> Instance = new EmptySet<T>();

        public bool IsProperSubsetOf(IEnumerable<T> other) => !other.Any();

        public bool IsProperSupersetOf(IEnumerable<T> other) => false;

        public bool IsSubsetOf(IEnumerable<T> other) => true;

        public bool IsSupersetOf(IEnumerable<T> other) => !other.Any();

        public bool Overlaps(IEnumerable<T> other) => false;

        public bool SetEquals(IEnumerable<T> other) => !other.Any();

        public bool Contains(T item) => false;

        public void CopyTo(T[] array, int arrayIndex) { }

        public int Count => 0;

        public bool IsReadOnly => true;

        bool ISet<T>.Add(T item) => throw new NotSupportedException("EmptySet is immutable.");

        void ICollection<T>.Add(T item) => throw new NotSupportedException("EmptySet is immutable.");

        void ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotSupportedException("EmptySet is immutable.");

        void ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotSupportedException("EmptySet is immutable.");

        void ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotSupportedException("EmptySet is immutable.");

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException("EmptySet is immutable.");

        void ICollection<T>.Clear() => throw new NotSupportedException("EmptySet is immutable.");

        bool ICollection<T>.Remove(T item) => throw new NotSupportedException("EmptySet is immutable.");

        public IEnumerator<T> GetEnumerator() => EmptyEnumerator<T>.Instance;

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private class EmptyEnumerator<TEmpty> : IEnumerator<TEmpty>
        {
            public static readonly EmptyEnumerator<TEmpty> Instance = new EmptyEnumerator<TEmpty>();

            public bool MoveNext() => false;

            public void Reset() { }

            TEmpty IEnumerator<TEmpty>.Current => throw new NotSupportedException("EmptySet has no elements.");
            public object Current => throw new NotSupportedException("EmptySet has no elements.");

            public void Dispose() { }
        }
    }
}
