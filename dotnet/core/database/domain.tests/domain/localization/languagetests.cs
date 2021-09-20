// <copyright file="LanguageTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public class LanguageTests : DomainTest
    {
        [Fact]
        public void GivenLanguageWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new LanguageBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithIsoCode("XX").Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithLocalisedName(new LocalisedTextBuilder(this.Session).WithLocale(new Locales(this.Session).FindBy(M.Locale.Name, Locales.EnglishGreatBritainName)).WithText("XXX)").Build());

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
