// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedItemBuilderExtensions.cs" company="Allors bvba">
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

namespace Allors.Domain.End2End
{
    public static partial class SerialisedItemBuilderExtensions
    {
        public static SerialisedItemBuilder WithDefaults(this SerialisedItemBuilder @this, ISession session, Config config)
        {
            if (config.End2End)
            {
                var serviceDate = config.faker.Date.Past(refDate: session.Now());

                @this.WithName(config.faker.Lorem.Word());
                @this.WithDescription(config.faker.Lorem.Words().ToString());
                @this.WithKeywords(config.faker.Lorem.Words().ToString());
                @this.WithAcquiredDate(config.faker.Date.Past(refDate: serviceDate));
                @this.WithLastServiceDate(serviceDate);
                @this.WithNextServiceDate(config.faker.Date.Future(refDate: serviceDate));
                @this.WithSerialNumber(config.faker.Random.AlphaNumeric(12));
                @this.WithSerialNumber(config.faker.Random.AlphaNumeric(12));

                foreach (Locale additionalLocale in session.GetSingleton().AdditionalLocales)
                {
                    @this.WithLocalisedName(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Word()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedDescription(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                    @this.WithLocalisedKeyword(new LocalisedTextBuilder(session).WithText(config.faker.Lorem.Words().ToString()).WithLocale(additionalLocale).Build());
                }
            }

            return @this;
        }
    }
}
