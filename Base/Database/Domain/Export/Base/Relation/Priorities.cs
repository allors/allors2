// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyRelationshipPriorities.cs" company="Allors bvba">
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

    public partial class Priorities
    {
        private static readonly Guid VeryHighId = new Guid("AE6AB101-481C-4ff1-8BE5-6CD6311903D7");
        private static readonly Guid HighId = new Guid("23248D29-A6B5-4081-A1B9-101A28460366");
        private static readonly Guid MediumId = new Guid("3B6A4A9A-1124-47fd-B812-DD034BE193E4");
        private static readonly Guid LowId = new Guid("ED1E1A54-343D-42d4-A1C3-884C7D925372");
        private static readonly Guid FirstId = new Guid("9638E638-1DCE-4f51-B6AF-598CE968313C");
        private static readonly Guid SecondId = new Guid("1BE83C5B-72C4-4d08-900B-79D2EF36BF1A");
        private static readonly Guid ThirdId = new Guid("1078C4C8-37B4-4f5b-B650-04DEA2C337C8");

        private UniquelyIdentifiableSticky<Priority> cache;

        public Priority VeryHigh => this.Cache[VeryHighId];

        public Priority High => this.Cache[HighId];

        public Priority Medium => this.Cache[MediumId];

        public Priority Low => this.Cache[LowId];

        public Priority First => this.Cache[FirstId];

        public Priority Second => this.Cache[SecondId];

        public Priority Third => this.Cache[ThirdId];

        private UniquelyIdentifiableSticky<Priority> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Priority>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PriorityBuilder(this.Session)
                .WithName("Very High")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoog").WithLocale(dutchLocale).Build())
                .WithUniqueId(VeryHighId)
                .WithIsActive(true)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("High")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoog").WithLocale(dutchLocale).Build())
                .WithUniqueId(HighId)
                .WithIsActive(true)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Medium")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gemiddeld").WithLocale(dutchLocale).Build())
                .WithUniqueId(MediumId)
                .WithIsActive(true)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Low")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laag").WithLocale(dutchLocale).Build())
                .WithUniqueId(LowId)
                .WithIsActive(true)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("First")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Eerste").WithLocale(dutchLocale).Build())
                .WithUniqueId(FirstId)
                .WithIsActive(true)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Third")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Tweede").WithLocale(dutchLocale).Build())
                .WithUniqueId(SecondId)
                .WithIsActive(true)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Third")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Derde").WithLocale(dutchLocale).Build())
                .WithUniqueId(ThirdId)
                .WithIsActive(true)
                .Build();
        }
    }
}
