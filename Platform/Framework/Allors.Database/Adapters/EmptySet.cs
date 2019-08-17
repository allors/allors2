// <copyright file="EmptySet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters
{
    using System.Collections;
    using System.Collections.Generic;

    public class EmptySet<T> : ISet<T>
    {
        private static readonly HashSet<T> StaticEmptySet = new HashSet<T>();

        public IEnumerator<T> GetEnumerator() => StaticEmptySet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => StaticEmptySet.GetEnumerator();

        public void UnionWith(IEnumerable<T> other) => throw new System.NotSupportedException();

        public void IntersectWith(IEnumerable<T> other) => throw new System.NotSupportedException();

        public void ExceptWith(IEnumerable<T> other) => throw new System.NotSupportedException();

        public void SymmetricExceptWith(IEnumerable<T> other) => throw new System.NotSupportedException();

        public bool IsSubsetOf(IEnumerable<T> other) => StaticEmptySet.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other) => StaticEmptySet.IsSupersetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other) => StaticEmptySet.IsProperSupersetOf(other);

        public bool IsProperSubsetOf(IEnumerable<T> other) => StaticEmptySet.IsProperSubsetOf(other);

        public bool Overlaps(IEnumerable<T> other) => StaticEmptySet.Overlaps(other);

        public bool SetEquals(IEnumerable<T> other) => StaticEmptySet.SetEquals(other);

        public bool Add(T item) => throw new System.NotSupportedException();

        void ICollection<T>.Add(T item) => throw new System.NotSupportedException();

        public void Clear() => throw new System.NotSupportedException();

        public bool Contains(T item) => StaticEmptySet.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => StaticEmptySet.CopyTo(array, arrayIndex);

        public bool Remove(T item) => throw new System.NotSupportedException();

        public int Count => StaticEmptySet.Count;

        public bool IsReadOnly => true;
    }
}
