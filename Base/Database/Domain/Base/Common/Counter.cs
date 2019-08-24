// <copyright file="Counter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Counter
    {
        public int NextElfProefValue() => Counters.NextElfProefValue(this.Strategy.Session, this.UniqueId);
    }
}
