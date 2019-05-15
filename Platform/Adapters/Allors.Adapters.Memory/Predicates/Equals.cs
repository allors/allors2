// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Equals.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Memory
{
    using Adapters;

    internal sealed class Equals : Predicate
    {
        private readonly IObject equals;

        internal Equals(IObject equals)
        {
            PredicateAssertions.ValidateEquals(equals);
            this.equals = equals;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            if (this.equals == null)
            {
                return ThreeValuedLogic.False;
            }

            return this.equals.Equals(strategy.GetObject())
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}