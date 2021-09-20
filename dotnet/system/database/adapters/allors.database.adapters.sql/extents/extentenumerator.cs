// <copyright file="ExtentEnumerator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Collections;
    using System.Collections.Generic;

    internal class ExtentEnumerator : IEnumerator
    {
        private readonly IEnumerator enumerator;

        internal ExtentEnumerator(IEnumerable<Reference> references) => this.enumerator = references.GetEnumerator();

        public object Current
        {
            get
            {
                var reference = (Reference)this.enumerator.Current;
                if (reference != null)
                {
                    return reference.Strategy.GetObject();
                }

                return null;
            }
        }

        public bool MoveNext() => this.enumerator.MoveNext();

        public void Reset() => this.enumerator.Reset();
    }
}
