// <copyright file="Or.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    internal sealed class Or : CompositePredicate
    {
        public Or(ExtentFiltered extent)
            : base(extent)
        {
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var unknown = false;
            foreach (var filter in this.Filters)
            {
                if (filter.Include)
                {
                    switch (filter.Evaluate(strategy))
                    {
                        case ThreeValuedLogic.True:
                            return ThreeValuedLogic.True;

                        case ThreeValuedLogic.Unknown:
                            unknown = true;
                            break;
                    }
                }
            }

            if (unknown)
            {
                return ThreeValuedLogic.Unknown;
            }

            return ThreeValuedLogic.False;
        }
    }
}
