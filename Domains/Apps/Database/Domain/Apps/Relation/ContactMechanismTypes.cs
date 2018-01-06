// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactMechanismTypes.cs" company="Allors bvba">
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

    public partial class ContactMechanismTypes
    {
        private static readonly Guid PhoneId = new Guid("443EE069-2975-4FA3-88DF-C243171379FD");
        private static readonly Guid MobilePhoneId = new Guid("CFA76E30-42A9-46A6-8D0F-A3D1D7907743");
        private static readonly Guid FaxId = new Guid("7AC12B75-061D-4249-862B-49C38CD233DF");

        private UniquelyIdentifiableSticky<ContactMechanismType> cache;

        public ContactMechanismType Phone => this.Cache[PhoneId];

        public ContactMechanismType MobilePhone => this.Cache[MobilePhoneId];

        public ContactMechanismType Fax => this.Cache[FaxId];

        private UniquelyIdentifiableSticky<ContactMechanismType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<ContactMechanismType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ContactMechanismTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Phone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Telefoon").WithLocale(dutchLocale).Build())
                .WithUniqueId(PhoneId)
                .Build();

            new ContactMechanismTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mobile Phone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Mobiel").WithLocale(dutchLocale).Build())
                .WithUniqueId(MobilePhoneId)
                .Build();

            new ContactMechanismTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fax").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(FaxId)
                .Build();
        }
    }
}
