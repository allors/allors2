// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ordinals.cs" company="Allors bvba">
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

    public partial class Ordinals
    {
        private static readonly Guid FirstId = new Guid("E312891F-7744-43ba-A69F-13878B1FC66B");
        private static readonly Guid SecondId = new Guid("6593FE82-A00F-4de6-9516-D652FE28A3EA");
        private static readonly Guid ThirdId = new Guid("C207121C-B534-4764-9724-3E829E9C9F21");

        private UniquelyIdentifiableSticky<Ordinal> cache;

        public Ordinal First => this.Cache[FirstId];

        public Ordinal Second => this.Cache[SecondId];

        public Ordinal Third => this.Cache[ThirdId];

        private UniquelyIdentifiableSticky<Ordinal> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Ordinal>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OrdinalBuilder(this.Session)
                .WithName("First")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Eerste").WithLocale(dutchLocale).Build())
                .WithUniqueId(FirstId)
                .WithIsActive(true)
                .Build();
            
            new OrdinalBuilder(this.Session)
                .WithName("Second")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Tweede").WithLocale(dutchLocale).Build())
                .WithUniqueId(SecondId)
                .WithIsActive(true)
                .Build();
            
            new OrdinalBuilder(this.Session)
                .WithName("Third")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Derde").WithLocale(dutchLocale).Build())
                .WithUniqueId(ThirdId)
                .WithIsActive(true)
                .Build();
        }
    }
}
