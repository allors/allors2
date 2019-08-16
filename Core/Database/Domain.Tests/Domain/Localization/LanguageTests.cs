// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageTests.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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