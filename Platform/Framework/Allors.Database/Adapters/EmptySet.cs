// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptySet.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System.Collections;
    using System.Collections.Generic;

    public class EmptySet<T> : ISet<T>
    {
        private static readonly HashSet<T> StaticEmptySet = new HashSet<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return StaticEmptySet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StaticEmptySet.GetEnumerator();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return StaticEmptySet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return StaticEmptySet.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return StaticEmptySet.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return StaticEmptySet.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return StaticEmptySet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return StaticEmptySet.SetEquals(other);
        }

        public bool Add(T item)
        {
            throw new System.NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new System.NotSupportedException();
        }

        public void Clear()
        {
            throw new System.NotSupportedException();
        }

        public bool Contains(T item)
        {
            return StaticEmptySet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            StaticEmptySet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new System.NotSupportedException();
        }

        public int Count
        {
            get
            {
                return StaticEmptySet.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
    }
}
