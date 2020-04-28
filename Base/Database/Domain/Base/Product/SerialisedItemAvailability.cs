// <copyright file="SerialisedItemAvailability.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SerialisedItemAvailability
    {
        public bool IsAvailable => this.Equals(new SerialisedItemAvailabilities(this.Strategy.Session).Available);

        public bool IsSold => this.Equals(new SerialisedItemAvailabilities(this.Strategy.Session).Sold);

        public bool IsInRent => this.Equals(new SerialisedItemAvailabilities(this.Strategy.Session).InRent);
    }
}
