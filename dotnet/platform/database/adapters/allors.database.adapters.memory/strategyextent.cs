// <copyright file="StrategyExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    public abstract class StrategyExtent : Allors.Extent
    {
        internal abstract Session Session { get; }

        internal abstract void UpgradeTo(ExtentFiltered extentFiltered);
    }
}
