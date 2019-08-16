//------------------------------------------------------------------------------------------------- 
// <copyright file="LazySet.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the AssociationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Meta
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class LazySet<T> : IEnumerable<T>
    {
        private readonly IList<T> array;
        private HashSet<T> set;

        internal LazySet(IEnumerable<T> collection)
        {
            this.array = collection.ToArray();
        }

        public int Count
        {
            get
            {
                return this.array.Count;
            }
        }

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

        public IEnumerator<T> GetEnumerator()
        {
            return this.array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
