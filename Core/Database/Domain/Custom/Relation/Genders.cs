// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Genders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class Genders
    {
        private static readonly Guid MaleId = new Guid("DAB59C10-0D45-4478-A802-3ABE54308CCD");
        private static readonly Guid FemaleId = new Guid("B68704AD-82F1-4d5d-BBAF-A54635B5034F");
        private static readonly Guid OtherId = new Guid("09210D7C-804B-4E76-AD91-0E150D36E86E");

        private UniquelyIdentifiableSticky<Gender> cache;

        public Gender Male => this.Cache[MaleId];

        public Gender Female => this.Cache[FemaleId];

        public Gender Other => this.Cache[OtherId];

        private UniquelyIdentifiableSticky<Gender> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Gender>(this.Session));

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new GenderBuilder(this.Session)
                .WithName("Male")
                .WithUniqueId(MaleId)
                .WithIsActive(true)
                .Build();

            new GenderBuilder(this.Session)
                .WithName("Female")
                .WithUniqueId(FemaleId)
                .WithIsActive(true)
                .Build();

            new GenderBuilder(this.Session)
                .WithName("Other")
                .WithUniqueId(FemaleId)
                .WithIsActive(true)
                .Build();
        }
    }
}
