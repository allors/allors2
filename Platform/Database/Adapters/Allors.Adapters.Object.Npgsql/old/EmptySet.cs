// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptySet.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
        private HashSet<T> empty = new HashSet<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return this.empty.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.empty.GetEnumerator();
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
            return this.empty.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.empty.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.empty.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.empty.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return this.empty.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return this.empty.SetEquals(other);
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
            return this.empty.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.empty.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new System.NotSupportedException();
        }

        public int Count 
        { 
            get
            {
                return this.empty.Count;
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