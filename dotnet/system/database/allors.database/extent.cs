// <copyright file="Extent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Meta;

    /// <summary>
    /// The Extent of a <see cref="IObjectType"/> is the set of all objects that either
    /// - are of the specified <see cref="IObjectType"/>
    /// - inherit from the specified <see cref="IObjectType"/>
    /// The extent can be filtered based on predicates.
    /// </summary>
    [DebuggerTypeProxy(typeof(ExtentDebugView))]
    public abstract class Extent : IList, IEnumerable<IObject>
    {
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
        public abstract int Count { get; }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter is a top level AND filter. If you require an OR or a NOT filter
        /// then simply add it to this AND filter.</value>
        public abstract ICompositePredicate Filter { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
        public bool IsFixedSize => true;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => true;

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
        public virtual bool IsSynchronized => false;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
        public virtual object SyncRoot => this;

        /// <summary>
        /// Gets the object type of this extent.
        /// </summary>
        /// <value>The type of the Extent.</value>
        public abstract IComposite ObjectType { get; }

        /// <summary>
        /// Gets the <see cref="IObject"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <value>The <see cref="IObject"/> at the specified index.</value>
        /// <returns>The object at the specified index.</returns>
        public IObject this[int index] => this.GetItem(index);

        /// <summary>
        /// Gets or sets the <see cref="object"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <value>The <see cref="object"/> at the specified index.</value>
        /// <returns>The object at the specified index.</returns>
        object IList.this[int index]
        {
            get => this.GetItem(index);
            set => throw new NotSupportedException("Extents are read only and fixed in size.");
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="IObject"/> to <see cref="Extent"/>.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Extent(IObject[] objects)
        {
            if (objects == null)
            {
                return null;
            }

            return new AllorsExtentConverted(objects);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Extent"/> to <see cref="IObject"/>.
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator IObject[](Extent extent) => extent?.ToArray();

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"></see>.
        /// This method is not supported.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to add to the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        public int Add(object value) => throw new NotSupportedException("Extents are read only and fixed in size.");

        /// <summary>
        /// Adds sorting based on the specified relation type.
        /// </summary>
        /// <param name="roleType">The role type by which to sort.</param>
        /// <returns>The current extent.</returns>
        public abstract Extent AddSort(IRoleType roleType);

        /// <summary>
        /// Adds sorting based on the specified role type and direction.
        /// </summary>
        /// <param name="roleType">The role type by which to sort.</param>
        /// <param name="direction">The sort direction.</param>
        /// <returns>The current extent.</returns>
        public abstract Extent AddSort(IRoleType roleType, SortDirection direction);

        /// <summary>
        /// Adds sorting based on the specified sort specification.
        /// </summary>
        /// <param name="sort">The sort specification.</param>
        /// <returns>The current extent.</returns>
        public Extent AddSort(Sort sort) => this.AddSort(sort.RoleType, sort.Direction);

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList"></see>.
        /// This method is not supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only. </exception>
        public void Clear() => throw new NotSupportedException("Extents are read only and fixed in size.");

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
        /// </returns>
        public abstract bool Contains(object value);

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
        public abstract void CopyTo(Array array, int index);

        IEnumerator<IObject> IEnumerable<IObject>.GetEnumerator()
        {
            foreach (var @object in this)
            {
                yield return (IObject)@object;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        public abstract IEnumerator GetEnumerator();

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// The index of value if found in the list; otherwise, -1.
        /// </returns>
        public abstract int IndexOf(object value);

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
        /// This method is not supported.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">value is null reference in the <see cref="T:System.Collections.IList"></see>.</exception>
        public void Insert(int index, object value) => throw new NotSupportedException("Extents are read only and fixed in size.");

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"></see>.
        /// This method is not supported.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to remove from the <see cref="T:System.Collections.IList"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        public void Remove(object value) => throw new NotSupportedException("Extents are read only and fixed in size.");

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"></see> item at the specified index.
        /// This method is not supported.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        public void RemoveAt(int index) => throw new NotSupportedException("Extents are read only and fixed in size.");

        /// <summary>
        /// Gets the extent as an array.
        /// </summary>
        /// <returns>An array containing the objects of this extent.</returns>
        public abstract IObject[] ToArray();

        /// <summary>
        /// Gets the extent as an array.
        /// </summary>
        /// <param name="type">The type of the array.</param>
        /// <returns>An array containing the objects of this extent.</returns>
        public abstract IObject[] ToArray(Type type);

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item at the specified index.</returns>
        protected abstract IObject GetItem(int index);

        /// <summary>
        /// A adapter extent that wraps an array of allors objects.
        /// </summary>
        private sealed class AllorsExtentConverted : Extent
        {
            /// <summary>
            /// The objects.
            /// </summary>
            private readonly IObject[] objects;

            /// <summary>
            /// Initializes a new state of the AllorsExtentConverted class.
            /// </summary>
            /// <param name="objects">The objects.</param>
            internal AllorsExtentConverted(IObject[] objects)
            {
                if (objects == null)
                {
                    this.objects = Array.Empty<IObject>();
                }
                else
                {
                    this.objects = (IObject[])objects.Clone();
                }
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
            /// </summary>
            /// <value></value>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
            public override int Count => this.objects.Length;

            /// <summary>
            /// Gets the filter.
            /// </summary>
            /// <value>
            /// The filter is a top level AND filter. If you require an OR or a NOT filter
            /// then simply add it to this AND filter.
            /// </value>
            public override ICompositePredicate Filter => throw new NotSupportedException("A converted extent does not support a filter");

            /// <summary>
            /// Gets the object type of this extent.
            /// </summary>
            /// <value>The type of the Extent.</value>
            public override IComposite ObjectType => null;

            /// <summary>
            /// Adds sorting based on the specified relation type..
            /// </summary>
            /// <param name="roleType">The role type by which to sort.</param>
            /// <returns>The current extent.</returns>
            public override Extent AddSort(IRoleType roleType) => throw new NotSupportedException("A converted extent does not support sorting");

            /// <summary>
            /// Adds sorting based on the specified relation type..
            /// </summary>
            /// <param name="roleType">The role type by which to sort.</param>
            /// <param name="direction">The sort direction.</param>
            /// <returns>The current extent.</returns>
            public override Extent AddSort(IRoleType roleType, SortDirection direction) => throw new NotSupportedException("A converted extent does not support sorting");

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
            /// <returns>
            /// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
            /// </returns>
            public override bool Contains(object value) => this.IndexOf(value) >= 0;

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in array at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
            /// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
            /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
            public override void CopyTo(Array array, int index) => Array.Copy(this.objects, 0, array, 0, this.objects.Length);

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator GetEnumerator() => this.objects.GetEnumerator();

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
            /// </summary>
            /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
            /// <returns>
            /// The index of value if found in the list; otherwise, -1.
            /// </returns>
            public override int IndexOf(object value) => Array.IndexOf(this.objects, (IObject)value);

            /// <summary>
            /// Gets the extent as an array.
            /// </summary>
            /// <returns>
            /// An array containing the objects of this extent.
            /// </returns>
            public override IObject[] ToArray() => this.objects;

            /// <summary>
            /// Gets the extent as an array.
            /// </summary>
            /// <param name="type">The type of the array.</param>
            /// <returns>
            /// An array containing the objects of this extent.
            /// </returns>
            public override IObject[] ToArray(Type type)
            {
                var typedArray = (IObject[])Array.CreateInstance(type, this.objects?.Length ?? 0);
                this.objects?.CopyTo(typedArray, 0);
                return typedArray;
            }

            /// <summary>
            /// Gets the item at the specified index.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>The item at the specified index.</returns>
            protected override IObject GetItem(int index) => this.objects[index];
        }

        /// <summary>
        /// The extent debug view.
        /// </summary>
        private class ExtentDebugView
        {
            /// <summary>
            /// The extent.
            /// </summary>
            private readonly Extent extent;

            /// <summary>
            /// Initializes a new state of the <see cref="ExtentDebugView"/> class.
            /// </summary>
            /// <param name="extent">
            /// The extent.
            /// </param>
            public ExtentDebugView(Extent extent) => this.extent = extent;

            /// <summary>
            /// Gets the values.
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public IObject[] Values => this.extent.ToArray();
        }
    }
}
