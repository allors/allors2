// <copyright file="IrpfRates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class IrpfRates
    {
        public static readonly Guid TenId = new Guid("8d7ba239-caf8-41e8-a6b8-cbf10fc7223f");

        private UniquelyIdentifiableSticky<IrpfRate> cache;

        public IrpfRate Ten => this.Cache[TenId];

        private UniquelyIdentifiableSticky<IrpfRate> Cache => this.cache ??= new UniquelyIdentifiableSticky<IrpfRate>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(TenId, v => v.Rate = 10);
        }
    }
}
