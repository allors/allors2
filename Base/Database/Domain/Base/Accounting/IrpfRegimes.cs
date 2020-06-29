// <copyright file="IrpfRegimes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class IrpfRegimes
    {
        public static readonly Guid Assessable19Id = new Guid("a82edb4e-dc92-4864-96fe-26ec6d1ef914");

        private UniquelyIdentifiableSticky<IrpfRegime> cache;

        public IrpfRegime Assessable19 => this.Cache[Assessable19Id];

        private UniquelyIdentifiableSticky<IrpfRegime> Cache => this.cache ??= new UniquelyIdentifiableSticky<IrpfRegime>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.IrpfRate);
        }

        protected override void BaseSetup(Setup setup)
        {
            var irpfRate19 = new IrpfRates(this.Session).nineteen;

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(Assessable19Id, v =>
            {
                v.Name = "IRPF Assessable 19%";
                localisedName.Set(v, dutchLocale, "IRPF-plichtig 19%");
                v.IrpfRate = irpfRate19;
                v.IsActive = true;
            });
        }
    }
}
