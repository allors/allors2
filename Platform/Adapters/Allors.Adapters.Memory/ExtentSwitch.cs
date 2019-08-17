// <copyright file="ExtentSwitch.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections;
    using Allors.Meta;

    public class ExtentSwitch : Allors.Extent
    {
        private readonly StrategyExtent strategyExtent;
        private Extent extent;

        public ExtentSwitch(Strategy strategy, IAssociationType associationType) => this.strategyExtent = new StrategyExtentAssociation(strategy, associationType);

        public ExtentSwitch(Strategy strategy, IRoleType roleType) => this.strategyExtent = new StrategyExtentRole(strategy, roleType);

        public override int Count
        {
            get
            {
                if (this.extent != null)
                {
                    return this.extent.Count;
                }

                return this.strategyExtent.Count;
            }
        }

        public override ICompositePredicate Filter
        {
            get
            {
                this.Upgrade();
                return this.extent.Filter;
            }
        }

        public override IObject First
        {
            get
            {
                if (this.extent != null)
                {
                    return this.extent.First;
                }

                return this.strategyExtent.First;
            }
        }

        public override IComposite ObjectType => this.strategyExtent.ObjectType;

        public Extent Extent
        {
            get
            {
                this.Upgrade();
                return this.extent;
            }
        }

        public override Allors.Extent AddSort(IRoleType roleType)
        {
            this.Upgrade();
            return this.extent.AddSort(roleType);
        }

        public override Allors.Extent AddSort(IRoleType roleType, SortDirection direction)
        {
            this.Upgrade();
            return this.extent.AddSort(roleType, direction);
        }

        public override bool Contains(object value)
        {
            if (this.extent != null)
            {
                return this.extent.Contains(value);
            }

            return this.strategyExtent.Contains(value);
        }

        public override void CopyTo(Array array, int index)
        {
            if (this.extent != null)
            {
                this.extent.CopyTo(array, index);
            }
            else
            {
                this.strategyExtent.CopyTo(array, index);
            }
        }

        public override IEnumerator GetEnumerator()
        {
            if (this.extent != null)
            {
                return this.extent.GetEnumerator();
            }

            return this.strategyExtent.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            if (this.extent != null)
            {
                return this.extent.IndexOf(value);
            }

            return this.strategyExtent.IndexOf(value);
        }

        public override IObject[] ToArray()
        {
            if (this.extent != null)
            {
                return this.extent.ToArray();
            }

            return this.strategyExtent.ToArray();
        }

        public override IObject[] ToArray(Type type)
        {
            if (this.extent != null)
            {
                return this.extent.ToArray(type);
            }

            return this.strategyExtent.ToArray(type);
        }

        protected override IObject GetItem(int index)
        {
            if (this.extent != null)
            {
                return this.extent[index];
            }

            return this.strategyExtent[index];
        }

        private void Upgrade()
        {
            if (this.extent == null)
            {
                this.extent = new ExtentFiltered(this.strategyExtent);
            }
        }
    }
}
