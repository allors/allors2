// <copyright file="Extent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsExtentMemory type.
// </summary>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public abstract class Extent : Allors.Database.Extent
    {
        private IObject[] defaultObjectArray;
        private Extent parent;

        protected Extent(Transaction transaction) => this.Transaction = transaction;

        public override int Count
        {
            get
            {
                this.Evaluate();
                return this.Strategies.Count;
            }
        }

        public Extent Parent
        {
            get => this.parent;

            set
            {
                if (this.parent != null)
                {
                    throw new ArgumentException("Extent has already a parent");
                }

                this.parent = value;
            }
        }

        internal Transaction Transaction { get; }

        internal ExtentSort Sorter { get; private set; }

        protected List<Strategy> Strategies { get; set; }

        public override Allors.Database.Extent AddSort(IRoleType roleType) => this.AddSort(roleType, SortDirection.Ascending);

        public override Allors.Database.Extent AddSort(IRoleType roleType, SortDirection direction)
        {
            if (this.Sorter == null)
            {
                this.Sorter = new ExtentSort(roleType, direction);
            }
            else
            {
                this.Sorter.AddSort(roleType, direction);
            }

            this.Invalidate();
            return this;
        }

        public override bool Contains(object value) => this.IndexOf(value) >= 0;

        public override void CopyTo(Array array, int index)
        {
            this.Evaluate();

            var i = index;
            foreach (var strategy in this.Strategies)
            {
                array.SetValue(strategy.GetObject(), i);
                ++i;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            this.Evaluate();
            return new ExtentEnumerator(this.Strategies.GetEnumerator());
        }

        public override int IndexOf(object value)
        {
            this.Evaluate();
            var containedObject = (IObject)value;

            var i = 0;
            foreach (var strategy in this.Strategies)
            {
                if (strategy.ObjectId.Equals(containedObject.Strategy.ObjectId))
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }

        public override IObject[] ToArray()
        {
            this.Evaluate();
            var clrType = this.Transaction.GetTypeForObjectType(this.ObjectType);

            if (this.Strategies.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(clrType, this.Strategies.Count);

                var i = 0;
                foreach (var strategy in this.Strategies)
                {
                    objects[i] = strategy.GetObject();
                    ++i;
                }

                return objects;
            }

            return this.defaultObjectArray ??= (IObject[])Array.CreateInstance(clrType, 0);
        }

        public override IObject[] ToArray(Type type)
        {
            this.Evaluate();
            if (this.Strategies.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(type, this.Strategies.Count);
                var i = 0;
                foreach (var strategy in this.Strategies)
                {
                    objects[i] = strategy.GetObject();
                    ++i;
                }

                return objects;
            }

            return (IObject[])Array.CreateInstance(type, 0);
        }

        internal bool ContainsStrategy(Strategy strategy)
        {
            this.Evaluate();
            return this.Strategies.Contains(strategy);
        }

        internal List<Strategy> GetEvaluatedStrategies()
        {
            this.Evaluate();
            return this.Strategies;
        }

        internal void Invalidate()
        {
            this.Strategies = null;
            this.parent?.Invalidate();
        }

        protected abstract void Evaluate();

        protected override IObject GetItem(int index)
        {
            this.Evaluate();
            return this.Strategies[index].GetObject();
        }
    }
}
