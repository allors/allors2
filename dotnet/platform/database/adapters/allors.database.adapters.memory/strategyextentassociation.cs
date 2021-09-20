// <copyright file="StrategyExtentAssociation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public sealed class StrategyExtentAssociation : StrategyExtent
    {
        private static readonly List<Strategy> EmptyList = new List<Strategy>();
        private readonly Strategy roleStrategy;
        private readonly IAssociationType associationType;

        private IObject[] defaultObjectArray;

        private List<Strategy> associations;

        public StrategyExtentAssociation(Strategy roleStrategy, IAssociationType associationType)
        {
            this.roleStrategy = roleStrategy;
            this.associationType = associationType;
        }

        public override int Count => this.GetStrategies().Count;

        public override ICompositePredicate Filter => throw new NotSupportedException();

        public override IObject First => this.GetStrategies().Select(strategy => strategy.GetObject()).FirstOrDefault();

        public override IComposite ObjectType => this.associationType.ObjectType;

        internal override Session Session => this.roleStrategy.MemorySession;

        public override Allors.Extent AddSort(IRoleType roleType) => throw new NotSupportedException();

        public override Allors.Extent AddSort(IRoleType roleType, SortDirection direction) => throw new NotSupportedException();

        public override bool Contains(object value)
        {
            var strategies = this.GetStrategies();
            var valueStrategy = this.roleStrategy.MemorySession.InstantiateMemoryStrategy(((IObject)value).Id);
            return strategies.Contains(valueStrategy);
        }

        // TODO: write tests
        public override void CopyTo(Array array, int index)
        {
            this.FillObjects();
            for (var i = 0; i < this.associations.Count; i++)
            {
                array.SetValue(this.associations[i].GetObject(), i + index);
            }
        }

        public override IEnumerator GetEnumerator()
        {
            this.FillObjects();
            return this.associations.Select(strategy => strategy.GetObject()).GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            this.FillObjects();
            var strategy = this.Session.InstantiateMemoryStrategy(((IObject)value).Id);
            return this.associations.IndexOf(strategy);
        }

        public override IObject[] ToArray()
        {
            this.FillObjects();
            var clrType = this.Session.GetTypeForObjectType(this.ObjectType);

            if (this.associations.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(clrType, this.associations.Count);
                this.CopyTo(objects, 0);
                return objects;
            }

            return this.defaultObjectArray ?? (this.defaultObjectArray = (IObject[])Array.CreateInstance(clrType, 0));
        }

        public override IObject[] ToArray(Type type)
        {
            this.FillObjects();
            if (this.associations.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(type, this.associations.Count);
                for (var i = 0; i < this.associations.Count; i++)
                {
                    objects[i] = this.associations[i].GetObject();
                }

                return objects;
            }

            return (IObject[])Array.CreateInstance(type, 0);
        }

        internal override void UpgradeTo(ExtentFiltered extent)
        {
            if (this.associationType.RoleType.IsMany)
            {
                extent.Filter.AddContains(this.associationType.RoleType, this.roleStrategy.GetObject());
            }
            else
            {
                extent.Filter.AddEquals(this.associationType.RoleType, this.roleStrategy.GetObject());
            }
        }

        protected override IObject GetItem(int index)
        {
            this.FillObjects();
            return this.associations[index].GetObject();
        }

        private void FillObjects()
        {
            if (this.associations == null)
            {
                var strategies = this.GetStrategies();

                if (strategies != null)
                {
                    this.associations = new List<Strategy>();
                    foreach (var strategy in strategies)
                    {
                        this.associations.Add(strategy);
                    }
                }
                else
                {
                    this.associations = EmptyList;
                }
            }
        }

        private List<Strategy> GetStrategies() => this.roleStrategy.GetStrategies(this.associationType);
    }
}
