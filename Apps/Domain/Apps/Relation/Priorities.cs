// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyRelationshipPriorities.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class Priorities
    {
        public static readonly Guid VeryHighId = new Guid("AE6AB101-481C-4ff1-8BE5-6CD6311903D7");
        public static readonly Guid HighId = new Guid("23248D29-A6B5-4081-A1B9-101A28460366");
        public static readonly Guid MediumId = new Guid("3B6A4A9A-1124-47fd-B812-DD034BE193E4");
        public static readonly Guid LowId = new Guid("ED1E1A54-343D-42d4-A1C3-884C7D925372");
        public static readonly Guid FirstId = new Guid("9638E638-1DCE-4f51-B6AF-598CE968313C");
        public static readonly Guid SecondId = new Guid("1BE83C5B-72C4-4d08-900B-79D2EF36BF1A");
        public static readonly Guid ThirdId = new Guid("1078C4C8-37B4-4f5b-B650-04DEA2C337C8");

        private UniquelyIdentifiableCache<Priority> cache;

        public Priority VeryHigh
        {
            get { return this.Cache.Get(VeryHighId); }
        }

        public Priority High
        {
            get { return this.Cache.Get(HighId); }
        }

        public Priority Medium
        {
            get { return this.Cache.Get(MediumId); }
        }

        public Priority Low
        {
            get { return this.Cache.Get(LowId); }
        }

        public Priority First
        {
            get { return this.Cache.Get(FirstId); }
        }

        public Priority Second
        {
            get { return this.Cache.Get(SecondId); }
        }

        public Priority Third
        {
            get { return this.Cache.Get(ThirdId); }
        }

        private UniquelyIdentifiableCache<Priority> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<Priority>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PriorityBuilder(this.Session)
                .WithName("Very High")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Very High").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoog").WithLocale(dutchLocale).Build())
                .WithUniqueId(VeryHighId)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("High")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("High").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoog").WithLocale(dutchLocale).Build())
                .WithUniqueId(HighId)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Medium")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Medium").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gemiddeld").WithLocale(dutchLocale).Build())
                .WithUniqueId(MediumId)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Low")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Low").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laag").WithLocale(dutchLocale).Build())
                .WithUniqueId(LowId)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("First")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("First").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Eerste").WithLocale(dutchLocale).Build())
                .WithUniqueId(FirstId)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Third")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Third").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Tweede").WithLocale(dutchLocale).Build())
                .WithUniqueId(SecondId)
                .Build();

            new PriorityBuilder(this.Session)
                .WithName("Third")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Third").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Derde").WithLocale(dutchLocale).Build())
                .WithUniqueId(ThirdId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
