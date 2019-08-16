// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentFiltered.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using Allors.Meta;

    internal sealed class ExtentFiltered : Extent
    {
        private readonly IComposite objectType;

        private And filter;

        internal ExtentFiltered(Session session, IComposite objectType)
            : base(session)
        {
            this.objectType = objectType;
        }

        internal ExtentFiltered(StrategyExtent extent)
            : base(extent.Session)
        {
            this.objectType = extent.ObjectType;
            extent.UpgradeTo(this);
        }

        public override ICompositePredicate Filter
        {
            get { return this.filter ?? (this.filter = new And(this)); }
        }

        public override IComposite ObjectType
        {
            get { return this.objectType; }
        }

        internal void CheckForAssociationType(IAssociationType association)
        {
            if (!this.objectType.ExistAssociationType(association))
            {
                throw new ArgumentException("Extent does not have association " + association);
            }
        }

        internal void CheckForRoleType(IRoleType roleType)
        {
            if (!this.objectType.ExistRoleType(roleType))
            {
                throw new ArgumentException("Extent does not have role " + roleType.SingularName);
            }
        }

        protected override void Evaluate()
        {
            if (this.Strategies == null)
            {
                this.Strategies = new List<Strategy>();

                HashSet<Strategy> allStrategies = this.Session.GetStrategiesForExtentIncludingDeleted(this.objectType);
                foreach (Strategy strategy in allStrategies)
                {
                    if (!strategy.IsDeleted)
                    {
                        if (this.filter == null || !this.filter.Include || this.filter.Evaluate(strategy) == ThreeValuedLogic.True)
                        {
                            this.Strategies.Add(strategy);
                        }
                    }
                }

                if (this.Sorter != null)
                {
                    this.Strategies.Sort(this.Sorter);
                }
            }
        }
    }
}
