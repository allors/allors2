// <copyright file="InventoryItemKind.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Meta;

    public partial class InventoryItemKind
    {
        public bool Serialised => this.Equals(new InventoryItemKinds(this.Strategy.Session).Serialised);

        public bool NonSerialised => this.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialised);
    }
}
