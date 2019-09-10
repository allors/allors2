// <copyright file="StrategyExtentRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public sealed class StrategyExtentRole : StrategyExtent
    {
        private static readonly List<Strategy> EmptyList = new List<Strategy>();

        private readonly Strategy associationStrategy;
        private readonly IRoleType roleType;

        private IObject[] defaultObjectArray;

        private List<Strategy> roles;

        public StrategyExtentRole(Strategy associationStrategy, IRoleType roleType)
        {
            this.associationStrategy = associationStrategy;
            this.roleType = roleType;
        }

        public override int Count => this.GetStrategies().Count;

        public override ICompositePredicate Filter => throw new NotSupportedException();

        public override IObject First => this.GetStrategies().Select(strategy => strategy.GetObject()).FirstOrDefault();

        public override IComposite ObjectType => (IComposite)this.roleType.ObjectType;

        internal override Session Session => this.associationStrategy.MemorySession;

        public override Allors.Extent AddSort(IRoleType sortRoleType) => throw new NotSupportedException();

        public override Allors.Extent AddSort(IRoleType subSortRoleType, SortDirection direction) => throw new NotSupportedException();

        public override bool Contains(object value)
        {
            var strategies = this.GetStrategies();
            var valueStrategy = this.associationStrategy.MemorySession.InstantiateMemoryStrategy(((IObject)value).Id);
            return strategies.Contains(valueStrategy);
        }

        // TODO: write test
        public override void CopyTo(Array array, int index)
        {
            this.FillObjects();
            for (var i = 0; i < this.roles.Count; i++)
            {
                array.SetValue(this.roles[i].GetObject(), i + index);
            }
        }

        public override IEnumerator GetEnumerator() => this.GetStrategies().ToList().Select(strategy => strategy.GetObject()).GetEnumerator();

        public override int IndexOf(object value)
        {
            this.FillObjects();
            var strategy = this.Session.InstantiateMemoryStrategy(((IObject)value).Id);
            return this.roles.IndexOf(strategy);
        }

        public override IObject[] ToArray()
        {
            this.FillObjects();
            var clrType = this.Session.GetTypeForObjectType(this.ObjectType);

            if (this.roles.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(clrType, this.roles.Count);
                this.CopyTo(objects, 0);
                return objects;
            }

            return this.defaultObjectArray ?? (this.defaultObjectArray = (IObject[])Array.CreateInstance(clrType, 0));
        }

        public override IObject[] ToArray(Type type)
        {
            this.FillObjects();
            if (this.roles.Count > 0)
            {
                var objects = (IObject[])Array.CreateInstance(type, this.roles.Count);
                for (var i = 0; i < this.roles.Count; i++)
                {
                    objects[i] = this.roles[i].GetObject();
                }

                return objects;
            }

            return (IObject[])Array.CreateInstance(type, 0);
        }

        internal override void UpgradeTo(ExtentFiltered extent)
        {
            if (this.roleType.AssociationType.IsMany)
            {
                extent.Filter.AddContains(this.roleType.AssociationType, this.associationStrategy.GetObject());
            }
            else
            {
                extent.Filter.AddEquals(this.roleType.AssociationType, this.associationStrategy.GetObject());
            }
        }

        protected override IObject GetItem(int index)
        {
            this.FillObjects();
            return this.roles[index].GetObject();
        }

        private void FillObjects()
        {
            if (this.roles == null)
            {
                var strategies = this.GetStrategies();

                if (strategies != null)
                {
                    this.roles = new List<Strategy>();
                    foreach (var strategy in strategies)
                    {
                        this.roles.Add(strategy);
                    }
                }
                else
                {
                    this.roles = EmptyList;
                }
            }
        }

        private List<Strategy> GetStrategies() => this.associationStrategy.GetStrategies(this.roleType);
    }
}
