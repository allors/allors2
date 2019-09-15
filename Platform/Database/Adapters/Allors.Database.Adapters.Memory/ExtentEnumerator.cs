// <copyright file="ExtentEnumerator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
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
                var currentStrategy = this.strategyEnumerator.Current;
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
