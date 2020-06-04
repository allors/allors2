// <copyright file="InventoryItemKind.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class InventoryItemKind
    {
        public bool IsSerialised => this.Equals(new InventoryItemKinds(this.Strategy.Session).Serialised);

        public bool IsNonSerialised => this.Equals(new InventoryItemKinds(this.Strategy.Session).NonSerialised);
    }
}
