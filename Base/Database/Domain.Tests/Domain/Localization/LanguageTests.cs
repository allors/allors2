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

namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class LanguageTests : DomainTest
    {
        [Test]
        public void GivenLanguageWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new LanguageBuilder(this.Session);
            builder.Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);
               
            builder.WithIsoCode("XX").Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);

            builder.WithLocalisedName(new LocalisedTextBuilder(this.Session).WithLocale(new Locales(this.Session).FindBy(M.Locale.Name, Locales.EnglishGreatBritainName)).WithText("XXX)").Build());
        
            Assert.IsFalse(this.Session.Derive().HasErrors);
        }
    }
}