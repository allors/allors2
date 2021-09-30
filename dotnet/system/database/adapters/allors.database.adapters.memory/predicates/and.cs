
// <copyright file="And.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    internal sealed class And : CompositePredicate
    {
        public And(ExtentFiltered extent) : base(extent)
        {
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            foreach (var filter in this.Filters)
            {
                if (filter.Include)
                {
                    switch (filter.Evaluate(strategy))
                    {
                        case ThreeValuedLogic.False:
                            return ThreeValuedLogic.False;

                        case ThreeValuedLogic.Unknown:
                            return ThreeValuedLogic.Unknown;
                    }
                }
            }

            return ThreeValuedLogic.True;
        }
    }
}
