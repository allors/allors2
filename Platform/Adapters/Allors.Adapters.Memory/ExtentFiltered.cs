
// <copyright file="ExtentFiltered.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
            : base(session) =>
            this.objectType = objectType;

        internal ExtentFiltered(StrategyExtent extent)
            : base(extent.Session)
        {
            this.objectType = extent.ObjectType;
            extent.UpgradeTo(this);
        }

        public override ICompositePredicate Filter => this.filter ?? (this.filter = new And(this));

        public override IComposite ObjectType => this.objectType;

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

                var allStrategies = this.Session.GetStrategiesForExtentIncludingDeleted(this.objectType);
                foreach (var strategy in allStrategies)
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
