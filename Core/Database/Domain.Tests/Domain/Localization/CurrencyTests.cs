//------------------------------------------------------------------------------------------------- 
// <copyright file="CurrencyTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>Defines the CurrencyTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;


    public class CurrencyTests : DomainTest
    {
        [Fact]
        public void GivenCurrencyWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new CurrencyBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithIsoCode("BND").Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder
                .WithLocalisedName(new LocalisedTextBuilder(this.Session)
                .WithText("Brunei Dollar")
                .WithLocale(new Locales(this.Session).FindBy(M.Locale.Name, Locales.EnglishGreatBritainName))
                .Build());

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
