// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenderTypes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class GenderTypes
    {
        private static readonly Guid MaleId = new Guid("DAB59C10-0D45-4478-A802-3ABE54308CCD");
        private static readonly Guid FemaleId = new Guid("B68704AD-82F1-4d5d-BBAF-A54635B5034F");
        private static readonly Guid OtherId = new Guid("09210D7C-804B-4E76-AD91-0E150D36E86E");

        private UniquelyIdentifiableSticky<GenderType> cache;

        public GenderType Male => this.Cache[MaleId];

        public GenderType Female => this.Cache[FemaleId];

        public GenderType Other => this.Cache[OtherId];

        private UniquelyIdentifiableSticky<GenderType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<GenderType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new GenderTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Male").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mannelijk").WithLocale(dutchLocale).Build())
                .WithUniqueId(MaleId)
                .Build();

            new GenderTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Female").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrouwelijk").WithLocale(dutchLocale).Build())
                .WithUniqueId(FemaleId)
                .Build();

            new GenderTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Other").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Anders").WithLocale(dutchLocale).Build())
                .WithUniqueId(FemaleId)
                .Build();
        }
    }
}
