// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentEnumerator.cs" company="Allors bvba">
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
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class ExtentEnumerator : IEnumerator<IObject>
    {
        private readonly IEnumerator<Strategy> strategyEnumerator;

        public ExtentEnumerator(IEnumerator<Strategy> strategyEnumerator) => this.strategyEnumerator = strategyEnumerator;

        public IObject Current
        {
            get
            {
                Strategy currentStrategy = this.strategyEnumerator.Current;
                if (currentStrategy == null)
                {
                    return null;
                }

                return currentStrategy.GetObject();
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() => this.strategyEnumerator.Dispose();

        public bool MoveNext() => this.strategyEnumerator.MoveNext();

        public void Reset() => this.strategyEnumerator.Reset();
    }
}
