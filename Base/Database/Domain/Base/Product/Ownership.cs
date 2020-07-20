// <copyright file="Ownership.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Ownership
    {
        public bool IsOwn => this.Equals(new Ownerships(this.Strategy.Session).Own);

        public bool IsTrading => this.Equals(new Ownerships(this.Strategy.Session).Trading);

        public bool IsThirdParty => this.Equals(new Ownerships(this.Strategy.Session).ThirdParty);
    }
}
