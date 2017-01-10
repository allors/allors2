// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrategyExtentRole.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

using Allors;

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

        public override int Count
        {
            get { return this.GetStrategies().Count; }
        }

        public override ICompositePredicate Filter
        {
            get { throw new NotSupportedException(); }
        }

        public override IObject First
        {
            get
            {
                return this.GetStrategies().Select(strategy => strategy.GetObject()).FirstOrDefault();
            }
        }

        public override IComposite ObjectType
        {
            get { return (IComposite)this.roleType.ObjectType; }
        }
        
        internal override Session Session
        {
            get { return this.associationStrategy.MemorySession; }
        }

        public override Allors.Extent AddSort(IRoleType sortRoleType)
        {
            throw new NotSupportedException();
        }

        public override Allors.Extent AddSort(IRoleType subSortRoleType, SortDirection direction)
        {
            throw new NotSupportedException();
        }

        public override bool Contains(object value)
        {
            var strategies = this.GetStrategies();
            Strategy valueStrategy = this.associationStrategy.MemorySession.InstantiateMemoryStrategy(((IObject)value).Id);
            return strategies.Contains(valueStrategy);
        }

        public override void CopyTo(Array array, int index)
        {
            this.FillObjects();
            for (int i = index; i < this.roles.Count; i++)
            {
                array.SetValue(this.roles[i].GetObject(), i);
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return this.GetStrategies().ToList().Select(strategy => strategy.GetObject()).GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            this.FillObjects();
            Strategy strategy = this.Session.InstantiateMemoryStrategy(((IObject)value).Id);
            return this.roles.IndexOf(strategy);
        }

        public override IObject[] ToArray()
        {
            this.FillObjects();
            Type clrType = this.Session.GetTypeForObjectType(this.ObjectType);

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
                for (int i = 0; i < this.roles.Count; i++)
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
                List<Strategy> strategies = this.GetStrategies();

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

        private List<Strategy> GetStrategies()
        {
            return this.associationStrategy.GetStrategies(this.roleType);
        }
    }
}