// <copyright file="SerializedItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class SerialisedItemState
    {
        public bool IsNA => this.Equals(new SerialisedItemStates(this.Strategy.Session).NA);

        public bool IsSold => this.Equals(new SerialisedItemStates(this.Strategy.Session).Sold);

        public bool IsInRent => this.Equals(new SerialisedItemStates(this.Strategy.Session).InRent);
    }
}
