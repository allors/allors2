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
        public static readonly Guid Assessable10Id = new Guid("a82edb4e-dc92-4864-96fe-26ec6d1ef914");
        public static readonly Guid ExemptId = new Guid("82986030-5E18-43c1-8CBE-9832ACD4151D");

        private UniquelyIdentifiableSticky<IrpfRegime> cache;

        public IrpfRegime Assessable10 => this.Cache[Assessable10Id];

        public IrpfRegime Exempt => this.Cache[ExemptId];

        private UniquelyIdentifiableSticky<IrpfRegime> Cache => this.cache ??= new UniquelyIdentifiableSticky<IrpfRegime>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.IrpfRate);
        }

        protected override void BaseSetup(Setup setup)
        {
            var irpfRate0 = new IrpfRates(this.Session).Ten;
            var irpfRate10 = new IrpfRates(this.Session).Ten;

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(Assessable10Id, v =>
            {
                v.Name = "IRPF Assessable 10%";
                localisedName.Set(v, dutchLocale, "IRPF-plichtig 10%");
                v.IrpfRate = irpfRate10;
                v.IsActive = true;
            });

            merge(ExemptId, v =>
            {
                v.Name = "Exempt";
                localisedName.Set(v, dutchLocale, "Vrijgesteld");
                v.IrpfRate = irpfRate0;
                v.IsActive = true;
            });
        }
    }
}
