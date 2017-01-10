// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extent.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the AllorsExtentMemory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public abstract class Extent : Allors.Extent
    {
        private readonly Session session;

        private IObject[] defaultObjectArray;
        private Extent parent;
        private ExtentSort sorter;

        private List<Strategy> strategies;

        protected Extent(Session session)
        {
            this.session = session;
        }

        public override int Count
        {
            get
            {
                this.Evaluate();
                return this.strategies.Count;
            }
        }

        public override IObject First
        {
            get
            {
                this.Evaluate();
                return (from Strategy strategy in this.Strategies select strategy.GetObject()).FirstOrDefault();
            }
        }

        public Extent Parent
        {
            get
            {
                return this.parent;
            }

            set
            {
                if (this.parent != null)
                {
                    throw new ArgumentException("Extent has already a parent");
                }

                this.parent = value;
            }
        }

        internal Session Session
        {
            get { return this.session; }
        }

        internal ExtentSort Sorter
        {
            get { return this.sorter; }
        }

        protected List<Strategy> Strategies
        {
            get { return this.strategies; }
            set { this.strategies = value; }
        }

        public override Allors.Extent AddSort(IRoleType roleType)
        {
            return this.AddSort(roleType, SortDirection.Ascending);
        }

        public override Allors.Extent AddSort(IRoleType roleType, SortDirection direction)
        {
            if (this.sorter == null)
            {
                this.sorter = new ExtentSort(roleType, direction);
            }
            else
            {
                this.sorter.AddSort(roleType, direction);
            }

            this.Invalidate();
            return this;
        }

        public override bool Contains(object value)
        {
            return this.IndexOf(value) >= 0;
        }

        public override void CopyTo(Array array, int index)
        {
            this.Evaluate();
            
            var i = index;
            foreach (var strategy in this.strategies)
            {
                array.SetValue(strategy.GetObject(), i);
                ++i;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            this.Evaluate();
            return new ExtentEnumerator(this.strategies.GetEnumerator());
        }

        public override int IndexOf(object value)
        {
            this.Evaluate();
            var containedObject = (IObject)value;

            var i = 0;
            foreach (var strategy in this.strategies)
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
            var clrType = this.Session.GetTypeForObjectType(this.ObjectType);

            if (this.strategies.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(clrType, this.strategies.Count);

                var i = 0;
                foreach (var strategy in this.strategies)
                {
                    objects[i] = strategy.GetObject();
                    ++i;
                }

                return objects;
            }

            return this.defaultObjectArray ?? (this.defaultObjectArray = (IObject[])Array.CreateInstance(clrType, 0));
        }

        public override IObject[] ToArray(Type type)
        {
            this.Evaluate();
            if (this.strategies.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(type, this.strategies.Count);
                var i = 0;
                foreach (var strategy in this.strategies)
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
            return this.strategies.Contains(strategy);
        }

        internal List<Strategy> GetEvaluatedStrategies()
        {
            this.Evaluate();
            return this.strategies;
        }

        internal void Invalidate()
        {
            this.strategies = null;
            if (this.parent != null)
            {
                this.parent.Invalidate();
            }
        }

        protected abstract void Evaluate();

        protected override IObject GetItem(int index)
        {
            this.Evaluate();
            return this.strategies[index].GetObject();
        }
    }
}