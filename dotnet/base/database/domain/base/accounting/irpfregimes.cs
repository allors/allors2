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
        public static readonly Guid Assessable15Id = new Guid("f6780ae0-39fc-459c-8ec0-d3c5a5fb066c");
        public static readonly Guid Assessable19Id = new Guid("a82edb4e-dc92-4864-96fe-26ec6d1ef914");
        public static readonly Guid ExemptId = new Guid("82986030-5E18-43c1-8CBE-9832ACD4151D");

        private UniquelyIdentifiableSticky<IrpfRegime> cache;

        public IrpfRegime Assessable15 => this.Cache[Assessable15Id];

        public IrpfRegime Assessable19 => this.Cache[Assessable19Id];

        public IrpfRegime Exempt => this.Cache[ExemptId];

        private UniquelyIdentifiableSticky<IrpfRegime> Cache => this.cache ??= new UniquelyIdentifiableSticky<IrpfRegime>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.IrpfRate);
        }

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(Assessable15Id, v =>
            {
                v.Name = "IRPF Assessable 15%";
                localisedName.Set(v, dutchLocale, "IRPF-plichtig 15%");
                v.IsActive = true;
            });
            var irpfregime = new IrpfRegimes(this.Session).FindBy(M.VatRegime.UniqueId, Assessable15Id);
            irpfregime.AddIrpfRate(new IrpfRates(this.Session).fifteen);

            merge(Assessable19Id, v =>
            {
                v.Name = "IRPF Assessable 19%";
                localisedName.Set(v, dutchLocale, "IRPF-plichtig 19%");
                v.IsActive = true;
            });
            irpfregime = new IrpfRegimes(this.Session).FindBy(M.VatRegime.UniqueId, Assessable19Id);
            irpfregime.AddIrpfRate(new IrpfRates(this.Session).nineteen);

            merge(ExemptId, v =>
            {
                v.Name = "Exempt";
                localisedName.Set(v, dutchLocale, "Vrijgesteld");
                v.IsActive = true;
            });
            irpfregime = new IrpfRegimes(this.Session).FindBy(M.VatRegime.UniqueId, ExemptId);
            irpfregime.AddIrpfRate(new IrpfRates(this.Session).Zero);
        }
    }
}
