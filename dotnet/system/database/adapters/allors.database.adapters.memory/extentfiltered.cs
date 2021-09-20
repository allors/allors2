// <copyright file="ExtentFiltered.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using Meta;

    internal sealed class ExtentFiltered : Extent
    {
        private readonly IComposite objectType;

        private And filter;

        internal ExtentFiltered(Transaction transaction, IComposite objectType)
            : base(transaction) =>
            this.objectType = objectType;

        public override ICompositePredicate Filter => this.filter ??= new And(this);

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

                foreach (var strategy in this.Transaction.GetStrategiesForExtentIncludingDeleted(this.objectType))
                {
                    if (!strategy.IsDeleted)
                    {
                        if (this.filter?.Include != true || this.filter.Evaluate(strategy) == ThreeValuedLogic.True)
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
