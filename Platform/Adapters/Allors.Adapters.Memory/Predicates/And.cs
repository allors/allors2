// --------------------------------------------------------------------------------------------------------------------
// <copyright file="And.cs" company="Allors bvba">
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
