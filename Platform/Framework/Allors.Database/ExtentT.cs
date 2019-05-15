//------------------------------------------------------------------------------------------------- 
// <copyright file="ExtentT.cs" company="Allors bvba">
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
// <summary>Defines the ExtentT type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Meta;

    /// <summary>
    /// The Extent of a <see cref="ObjectType"/> is the set of all objects that either
    /// - are of the specified <see cref="ObjectType"/>
    /// - inherit from the specified <see cref="ObjectType"/>
    /// The extent can be filter based on predicates.
    /// </summary>
    /// <typeparam name="T">The .Net type of the extent.</typeparam>
    public class Extent<T> : IList, IList<T> where T : IObject // Extent<T> must also implement IList to be a DataSource during DataBinding.
    {
        /// <summary>
        /// The non generic extent that will be a subject for this proxy.
        /// </summary>
        private readonly Extent extent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Extent{T}"/> class. 
        /// </summary>
        /// <param name="extent">
        /// The extent.
        /// </param>
        private Extent(Extent extent)
        {
            this.extent = extent;
        }
        
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        public int Count
        {
            get { return this.extent.Count; }
        }

        /// <summary>
        /// Gets the non generic extent.
        /// </summary>
        /// <value>The non generic extent.</value>
        public Extent BaseExtent
        {
            get { return this.extent; }
        }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter is a top level AND filter. If you require an OR or a NOT filter
        /// then simply add it to this AND filter.</value>
        public ICompositePredicate Filter
        {
            get { return this.extent.Filter; }
        }

        /// <summary>
        /// Gets the first object from the Extent.
        /// If there are no objects then null is returned.
        /// </summary>
        /// <value>The first.</value>
        public T First
        {
            get { return (T)this.extent.First; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        public bool IsFixedSize
        {
            get { return this.extent.IsFixedSize; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return this.extent.IsReadOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized
        {
            get { return this.extent.IsSynchronized; }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type of the Extent.</value>
        public IObjectType ObjectType
        {
            get { return this.extent.ObjectType; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot
        {
            get { return this.extent.SyncRoot; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        public T this[int index]
        {
            get { return (T)this.extent[index]; }
            set { throw new ArgumentException("Extent is readonly"); }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <value>The <see cref="System.Object"/> at the specified index</value>
        object IList.this[int index]
        {
            get { return this.extent[index]; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="IObject"/> to <see cref="Extent"/>.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Extent<T>(IObject[] objects)
        {
            if (objects == null)
            {
                return null;
            }

            return new Extent<T>(objects);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="IObject"/> to <see cref="Extent"/>.
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Extent<T>(Extent extent)
        {
            if (extent == null)
            {
                return null;
            }

            return new Extent<T>(extent);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Extent"/> to <see cref="IObject"/>.
        /// </summary>
        /// <param name="genericExtent">The extent.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Extent(Extent<T> genericExtent)
        {
            if (genericExtent == null)
            {
                return null;
            }

            return genericExtent.extent;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Extent"/> to <see cref="IObject"/>.
        /// </summary>
        /// <param name="genericExtent">The extent.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator T[](Extent<T> genericExtent)
        {
            if (genericExtent == null)
            {
                return null;
            }

            return genericExtent.ToArray();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Extent"/> to <see cref="IObject"/>.
        /// </summary>
        /// <param name="genericExtent">The extent.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator IObject[](Extent<T> genericExtent)
        {
            if (genericExtent == null)
            {
                return null;
            }

            return genericExtent.ToArray(typeof(T));
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public int Add(object value)
        {
            return this.extent.Add(value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(T item)
        {
            this.extent.Add(item);
        }

        /// <summary>
        /// Adds sorting based on the specified relation type..
        /// </summary>
        /// <param name="roleType">The role type by which to sort.</param>
        /// <returns>The current extent.</returns>
        public Extent<T> AddSort(IRoleType roleType)
        {
            this.extent.AddSort(roleType);
            return this;
        }

        /// <summary>
        /// Adds sorting based on the specified relation type..
        /// </summary>
        /// <param name="roleType">The role type by which to sort.</param>
        /// <param name="direction">The sort direction.</param>
        /// <returns>The current extent.</returns>
        public Extent<T> AddSort(IRoleType roleType, SortDirection direction)
        {
            this.extent.AddSort(roleType, direction);
            return this;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        public void Clear()
        {
            this.extent.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        public bool Contains(object value)
        {
            return this.extent.Contains(value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        public bool Contains(T item)
        {
            return this.extent.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///  <paramref name="array"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///  <paramref name="index"/> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///  <paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        public void CopyTo(Array array, int index)
        {
            this.extent.CopyTo(array, index);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-IObjectType T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.extent.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return this.extent.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object value)
        {
            return this.extent.IndexOf(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        public int IndexOf(T item)
        {
            return this.extent.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">
        /// <paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        public void Insert(int index, object value)
        {
            this.extent.Insert(index, value);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        public void Insert(int index, T item)
        {
            this.extent.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public void Remove(object value)
        {
            this.extent.Remove(value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(T item)
        {
            throw new ArgumentException("Extent is readonly");
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        public void RemoveAt(int index)
        {
            this.extent.RemoveAt(index);
        }

        /// <summary>
        /// Toes the array.
        /// </summary>
        /// <returns>The array.</returns>
        public T[] ToArray()
        {
            var result = new T[this.extent.Count];
            this.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Toes the array.
        /// </summary>
        /// <param name="type">The type .</param>
        /// <returns>The array.</returns>
        public IObject[] ToArray(Type type)
        {
            return this.extent.ToArray(type);
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < this.extent.Count; i++)
            {
                yield return (T)this.extent[i];
            }
        }
    }
}