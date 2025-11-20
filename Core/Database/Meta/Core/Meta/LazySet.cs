// <copyright file="LazySet.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Meta
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class LazySet<T> : IEnumerable<T>
    {
        private readonly IList<T> array;
        private HashSet<T> set;

        internal LazySet(IEnumerable<T> collection) => this.array = collection.ToArray();

        public int Count => this.array.Count;

        public bool Contains(T item)
        {
            if (this.array.Count > 0)
            {
                if (this.set == null)
                {
                    this.set = new HashSet<T>(this.array);
                }

                return this.set.Contains(item);
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator() => this.array.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
